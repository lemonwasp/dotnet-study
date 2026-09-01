# TIFF-to-PDF Conversion: Performance Optimization

> This case study is reconstructed from personal engineering experience. It excludes production source code, internal design documents, customer information, business data, and actual project files.

## Overview

The goal was to validate and improve a multi-page TIFF-to-PDF conversion flow in a .NET Framework application.

The first implementation proved that the conversion was technically possible, but its processing time increased noticeably with the number of TIFF pages. Profiling showed that the main cost came from pixel-by-pixel bitmap processing and intermediate image file I/O rather than PDF serialization itself.

## Initial Flow

```text
LibTiff
  → ReadRGBAImage
  → Bitmap.SetPixel(...)
  → temporary PNG file
  → XImage.FromFile(...)
  → PDF
```

This design had two expensive characteristics:

- `Bitmap.SetPixel(...)` performed a managed method call for every pixel.
- Each page was written to disk and read again before being added to the PDF.

### Baseline

| TIFF pages | File size | Processing time |
|---:|---:|---:|
| 3 | 130 KB | 2.27 s |
| 6 | 259 KB | 4.80 s |
| 9 | 388 KB | 6.35 s |
| 30 | 1,298 KB | 21.01 s |

The near-linear increase suggested a repeated per-page cost.

## Optimization

### 1. Bulk pixel transfer

Pixel-by-pixel processing was replaced with a locked bitmap buffer and a bulk memory copy.

```text
Bitmap.SetPixel(...)
        ↓
Bitmap.LockBits(...)
Marshal.Copy(...)
```

`LockBits` exposes the bitmap's underlying buffer, allowing decoded pixel data to be copied in bulk instead of crossing the managed API boundary once per pixel.

### 2. In-memory image pipeline

Temporary PNG save/read/delete operations were removed. Each page is encoded as JPEG in a `MemoryStream` and passed directly to PDFsharp.

```text
LibTiff
  → ReadRGBAImage
  → Bitmap (LockBits + Marshal.Copy)
  → JPEG MemoryStream
  → XImage.FromStream(...)
  → PDF
```

Because `XImage.FromStream(...)` may access the source lazily, the stream lifetime must cover the period in which the corresponding image is used and the PDF is saved.

## Results

| TIFF pages | File size | Before | After | Speed-up | Time reduction |
|---:|---:|---:|---:|---:|---:|
| 3 | 130 KB | 2.27 s | 0.571 s | 3.98× | 74.8% |
| 6 | 259 KB | 4.80 s | 1.031 s | 4.66× | 78.5% |
| 9 | 388 KB | 6.35 s | 1.623 s | 3.91× | 74.4% |
| 30 | 1,298 KB | 21.01 s | 4.535 s | 4.63× | 78.4% |

Across the measured cases, the optimized flow was approximately **4–4.7× faster**, reducing total processing time by approximately **74–79%**. Based on the combined measurements, total elapsed time fell from **34.43 s to 7.76 s**, an overall improvement of approximately **4.44×**.

## Remaining Bottleneck

After the main optimization, individual stages were measured again.

| Stage | Approximate time per page |
|---|---:|
| `ReadRGBAImage` | 80–100 ms |
| Bitmap creation and copy | 35–45 ms |
| `pdfDoc.Save(...)` | 0–5 ms |

LibTiff decoding is now the most expensive remaining operation. PDF serialization is negligible, and bitmap creation has already been reduced to a relatively small part of the total cost.

## Why Optimization Stopped Here

Further work on the LibTiff decoding stage may be technically possible, but the expected gain is small relative to the investigation and implementation effort.

The optimization was intentionally stopped because:

- the original high-cost operations were removed;
- the current implementation already meets the performance objective;
- PDF generation no longer contributes meaningfully to latency;
- the remaining bottleneck is primarily TIFF decoding;
- additional work would produce diminishing returns and increase complexity.

The final design balances performance, maintainability, implementation complexity, and development cost.

## Lessons Learned

- Measure stages independently before deciding what to optimize.
- Per-pixel managed calls can dominate image-processing workloads.
- Avoid unnecessary disk I/O when intermediate data can remain in memory.
- Stream lifetime is part of correctness when libraries may read lazily.
- The right stopping point is an engineering decision based on expected value, not the absence of further technical possibilities.
