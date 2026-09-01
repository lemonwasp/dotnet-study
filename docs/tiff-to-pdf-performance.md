# TIFF-to-PDF Conversion: From Test Data Generation to Performance Optimization

> This case study is reconstructed from personal engineering experience. It excludes production source code, internal design documents, customer information, business data, and actual project files.

## Overview

The requirement was to verify whether multi-page TIFF files could be converted reliably to PDF in a .NET Framework application and then make the conversion fast enough for practical use.

The work began before the converter itself could be evaluated. No suitable multi-page TIFF samples were available, so I first built a dedicated test-data generation program. I then used the generated files to validate the conversion flow, establish a reproducible benchmark, identify bottlenecks, and optimize the implementation.

```text
Build a multi-page TIFF generator
              ↓
Generate controlled test files
              ↓
Research and select image/PDF libraries
              ↓
Implement a TIFF-to-PDF proof of concept
              ↓
Measure the baseline
              ↓
Profile individual processing stages
              ↓
Remove per-pixel and disk I/O bottlenecks
              ↓
Measure again and evaluate further ROI
```

## 1. Building the Test Data Generator

### Problem

The technical validation required multi-page TIFF files, but no usable samples were provided. Depending on arbitrary files from the internet would also make the test conditions difficult to control and reproduce.

### Solution

I created a small program specifically for generating multi-page TIFF test data. Using this program, I produced TIFF files with different page counts and used the same controlled fixtures throughout development and benchmarking.

| Generated TIFF | File size |
|---:|---:|
| 3 pages | 130 KB |
| 6 pages | 259 KB |
| 9 pages | 388 KB |
| 30 pages | 1,298 KB |

This provided:

- reproducible inputs for technical validation;
- controlled variation in page count;
- data for observing how processing time scaled;
- a repeatable benchmark for comparing implementations.

Creating the test-data environment first made it possible to evaluate both correctness and performance without waiting for production files.

## 2. Technical Validation

Reference implementations for this exact multi-page TIFF and .NET Framework combination were limited, so I investigated libraries and built a proof of concept around:

- **LibTiff** for reading and decoding individual TIFF pages;
- **System.Drawing.Bitmap** for creating image data;
- **PDFsharp** for composing and saving the PDF.

The first objective was feasibility: read every page from a TIFF file, preserve the multi-page structure, and write the pages into a PDF.

### Initial Flow

```text
LibTiff
  → ReadRGBAImage
  → Bitmap.SetPixel(...)
  → temporary PNG file
  → XImage.FromFile(...)
  → PDF
```

The implementation successfully produced PDFs, confirming that the required conversion was possible. The next step was to measure whether its performance was acceptable.

## 3. Baseline Measurement

| TIFF pages | File size | Processing time |
|---:|---:|---:|
| 3 | 130 KB | 2.27 s |
| 6 | 259 KB | 4.80 s |
| 9 | 388 KB | 6.35 s |
| 30 | 1,298 KB | 21.01 s |

Processing time increased almost linearly with page count, indicating a repeated per-page cost.

Profiling identified two expensive operations:

- `Bitmap.SetPixel(...)` performed a managed method call for every pixel.
- Each page was saved as a temporary PNG, read again, and deleted before being added to the PDF.

PDF serialization itself was not the main problem.

## 4. Optimization

### 4.1 Bulk Pixel Transfer

Pixel-by-pixel bitmap construction was replaced with a locked bitmap buffer and a bulk memory copy.

```text
Bitmap.SetPixel(...)
        ↓
Bitmap.LockBits(...)
Marshal.Copy(...)
```

`LockBits` exposes the bitmap's underlying buffer, allowing decoded pixel data to be copied in bulk instead of crossing the managed API boundary once per pixel.

### 4.2 In-Memory Image Pipeline

The temporary PNG save/read/delete cycle was removed. Each page is now encoded as JPEG in a `MemoryStream` and passed directly to PDFsharp.

### Final Flow

```text
LibTiff
  → ReadRGBAImage
  → Bitmap (LockBits + Marshal.Copy)
  → JPEG MemoryStream
  → XImage.FromStream(...)
  → PDF
```

Because `XImage.FromStream(...)` may access its input lazily, the source stream remains alive until the corresponding image has been used and the PDF has been saved.

## 5. Results

| TIFF pages | File size | Before | After | Speed-up | Time reduction |
|---:|---:|---:|---:|---:|---:|
| 3 | 130 KB | 2.27 s | 0.571 s | 3.98× | 74.8% |
| 6 | 259 KB | 4.80 s | 1.031 s | 4.66× | 78.5% |
| 9 | 388 KB | 6.35 s | 1.623 s | 3.91× | 74.4% |
| 30 | 1,298 KB | 21.01 s | 4.535 s | 4.63× | 78.4% |

Across the measured cases, the optimized flow was approximately **4–4.7× faster**, reducing total processing time by approximately **74–79%**.

Across all four benchmarks, total elapsed time fell from **34.43 s to 7.76 s**, an overall improvement of approximately **4.44×**.

## 6. Remaining Bottleneck

After the main optimization, individual stages were measured again.

| Stage | Approximate time per page |
|---|---:|
| `ReadRGBAImage` | 80–100 ms |
| Bitmap creation and copy | 35–45 ms |
| `pdfDoc.Save(...)` | 0–5 ms |

LibTiff decoding is now the most expensive remaining operation. PDF serialization is negligible, and bitmap construction has already been reduced to a relatively small part of the total cost.

## 7. Why Optimization Stopped Here

Further optimization of the LibTiff decoding stage may be technically possible, but the expected gain is small relative to the required investigation and implementation effort.

The optimization was intentionally stopped because:

- the original high-cost operations were removed;
- the current implementation already achieved approximately a fourfold improvement;
- PDF generation no longer contributes meaningfully to total latency;
- the remaining bottleneck is primarily TIFF decoding;
- additional work would produce diminishing returns and increase complexity.

The final design balances performance, maintainability, implementation complexity, and development cost.

## Lessons Learned

- When required test data is unavailable, building a generator can be part of the engineering solution.
- Controlled fixtures make technical validation and performance comparisons reproducible.
- Prove feasibility first, then measure before optimizing.
- Per-pixel managed calls can dominate image-processing workloads.
- Avoid unnecessary disk I/O when intermediate data can remain in memory.
- Stream lifetime is part of correctness when a library may read lazily.
- The right stopping point is based on expected value, not the absence of further technical possibilities.
