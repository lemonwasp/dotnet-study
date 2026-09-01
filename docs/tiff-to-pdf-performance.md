# TIFF-to-PDF Conversion: From Test Data Generation to Performance Optimization

> This case study is reconstructed from personal engineering experience. It excludes production source code, internal design documents, customer information, business data, and actual project files.

## Overview

The requirement was to validate multi-page TIFF-to-PDF conversion in a .NET Framework application and improve it to a practical performance level.

No suitable multi-page TIFF samples were available at the start. I therefore built a test-data generator first, used its output to implement the initial converter, measured the result, and improved the design in multiple stages.

```text
Build a multi-page TIFF generator
              ↓
Generate controlled test files
              ↓
Research libraries and validate conversion
              ↓
Implement a file-based version
              ↓
Measure and identify disk I/O
              ↓
Move intermediate processing to memory
              ↓
Fix stream lifetime management
              ↓
Remove per-pixel bitmap overhead
              ↓
Measure the final result and evaluate further ROI
```

## 1. Building a Multi-Page TIFF Generator

### Problem

The technical validation required TIFF files containing multiple pages, but no usable samples were provided. Without controlled input files, it would also be difficult to reproduce tests or compare processing time by page count.

### Implementation

I created a dedicated program for generating multi-page TIFF files. It used the .NET image encoder's save flag with `EncoderValue.MultiFrame` to create the first frame, append subsequent frames, and finalize a single multi-page TIFF.

```text
Create first frame with EncoderValue.MultiFrame
                    ↓
Append additional page frames
                    ↓
Finalize the multi-frame TIFF
```

The generator produced the fixtures used throughout validation and benchmarking:

| Generated TIFF | File size |
|---:|---:|
| 3 pages | 130 KB |
| 6 pages | 259 KB |
| 9 pages | 388 KB |
| 30 pages | 1,298 KB |

This made the work reproducible and allowed performance to be compared under controlled changes in page count.

## 2. Library Research and Constraints

### Limited Reference Material

There were very few reference implementations for multi-page TIFF-to-PDF conversion in this .NET Framework environment. The work therefore had to begin with library research rather than adapting an established example.

The investigation focused on three separate responsibilities:

- reading every page from a multi-page TIFF;
- converting decoded pixel data into an image representation;
- creating a PDF and adding each image as a page.

### Licensing Constraint

The implementation also had to use libraries available under a free license suitable for the project. Common all-in-one PDF conversion libraries could not simply be adopted under this constraint.

Instead of using a dedicated TIFF-to-PDF converter, I selected **PDFsharp**, which is primarily a PDF generation library, and constructed the conversion pipeline explicitly:

- **LibTiff** reads and decodes the TIFF pages;
- **System.Drawing.Bitmap** holds the intermediate image data;
- **PDFsharp** creates the PDF document and places each converted image onto a page.

This increased the implementation work, but it satisfied the licensing constraint and kept each stage of the conversion under direct control.

## 3. Initial Technical Validation

With the library combination decided, I built a proof of concept. The first objective was to confirm that every TIFF page could be read and written into a PDF.

### Initial File-Based Flow

```text
TIFF
  → Bitmap
  → temporary PNG file
  → XImage.FromFile(...)
  → PDF
```

This implementation proved that the conversion was possible, but every page repeated the following operations:

```text
Create PNG
    → save file
    → read file again
    → add it to the PDF
    → delete file
```

As the TIFF page count increased, the cost of this cycle accumulated. Measurement showed that the primary bottleneck was **disk I/O**, not PDF serialization.

## 4. Baseline Measurement

| TIFF pages | File size | Processing time |
|---:|---:|---:|
| 3 | 130 KB | 2.27 s |
| 6 | 259 KB | 4.80 s |
| 9 | 388 KB | 6.35 s |
| 30 | 1,298 KB | 21.01 s |

The near-linear increase was consistent with an expensive file operation being repeated for every page.

## 5. First Improvement: File-Based to In-Memory Processing

The temporary PNG workflow was replaced with an in-memory pipeline.

### Before

```text
TIFF
  → Bitmap
  → temporary PNG file
  → XImage.FromFile(...)
  → PDF
```

### After

```text
TIFF
  → MemoryStream
  → XImage.FromStream(...)
  → PDF
```

The intermediate image no longer needed to be saved, reopened, and deleted for every TIFF page. This removed the dominant disk I/O from the conversion path.

## 6. Additional Problem: Stream Lifetime

Moving to `XImage.FromStream(...)` introduced an important object-lifetime issue.

Creating an `XImage` does not always mean the underlying stream can be released immediately. PDFsharp may still require the stream's data while drawing the image or saving the PDF. Disposing the `MemoryStream` before `pdfDoc.Save(...)` could therefore cause failures or invalid output.

The object-management strategy was changed so that:

1. each page's stream remained alive while its `XImage` was in use;
2. the PDF was completely saved;
3. the related images and streams were then disposed deterministically.

This was not only a performance change: correct resource lifetime became part of the converter's design.

## 7. Further Improvement: Bulk Bitmap Creation

After eliminating disk I/O, bitmap construction was optimized separately.

Pixel-by-pixel processing with `Bitmap.SetPixel(...)` was replaced with a locked bitmap buffer and a bulk memory copy:

```text
Bitmap.SetPixel(...)
        ↓
Bitmap.LockBits(...)
Marshal.Copy(...)
```

`LockBits` exposes the bitmap buffer so that the decoded pixel data can be copied in bulk, avoiding a managed method call for every pixel.

### Final Processing Flow

```text
LibTiff
  → ReadRGBAImage
  → Bitmap (LockBits + Marshal.Copy)
  → JPEG MemoryStream
  → XImage.FromStream(...)
  → PDF
```

## 8. Final Results

| TIFF pages | File size | Before | After | Speed-up | Time reduction |
|---:|---:|---:|---:|---:|---:|
| 3 | 130 KB | 2.27 s | 0.571 s | 3.98× | 74.8% |
| 6 | 259 KB | 4.80 s | 1.031 s | 4.66× | 78.5% |
| 9 | 388 KB | 6.35 s | 1.623 s | 3.91× | 74.4% |
| 30 | 1,298 KB | 21.01 s | 4.535 s | 4.63× | 78.4% |

Across the measured cases, the final flow was approximately **4–4.7× faster**, reducing processing time by approximately **74–79%**.

Across all four benchmarks, total elapsed time fell from **34.43 s to 7.76 s**, an overall improvement of approximately **4.44×**.

## 9. Remaining Bottleneck

After the main improvements, individual stages were measured again.

| Stage | Approximate time per page |
|---|---:|
| `ReadRGBAImage` | 80–100 ms |
| Bitmap creation and copy | 35–45 ms |
| `pdfDoc.Save(...)` | 0–5 ms |

LibTiff decoding is now the largest remaining cost. PDF serialization is negligible, while bitmap construction has already been reduced to a relatively small part of total processing time.

## 10. Why Optimization Stopped Here

Further optimization of LibTiff decoding may be technically possible, but the expected gain is small relative to the required investigation and implementation effort.

The work was intentionally stopped because:

- the repeated disk I/O was eliminated;
- the per-pixel bitmap overhead was removed;
- the implementation already achieved approximately a fourfold improvement;
- PDF saving no longer contributes meaningfully to latency;
- the remaining bottleneck is primarily TIFF decoding;
- additional optimization would produce diminishing returns and increase complexity.

The final design balances performance, correctness, resource management, maintainability, and development cost.

## Lessons Learned

- When test data is unavailable, building a generator can be part of the engineering solution.
- `EncoderValue.MultiFrame` makes controlled multi-page TIFF fixtures possible.
- Controlled fixtures make technical validation and benchmarks reproducible.
- Licensing is an architectural constraint and can determine whether a ready-made converter is usable.
- When no all-in-one library fits, separating decoding, image processing, and PDF generation provides a workable design.
- Prove feasibility first, then measure before optimizing.
- Repeated temporary-file operations can dominate a page-based processing pipeline.
- In-memory processing removes I/O but introduces resource-lifetime responsibilities.
- Per-pixel managed calls can dominate bitmap construction.
- The correct stopping point is based on expected value, not the absence of further technical possibilities.
