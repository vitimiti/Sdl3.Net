// The MIT License
//
// Copyright © 2025 Victor Matia <vmatir@outlook.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

namespace Sdl3.Net.Video.Blending;

/// <summary>
/// Represents the blend mode used for rendering operations.
/// </summary>
public enum BlendMode : uint
{
    /// <summary>
    /// No blending mode is applied.
    /// </summary>
    /// <remarks>dstRGBA = srcRGBA</remarks>
    None = 0x00000000U,

    /// <summary>
    /// Blends the source pixel with the destination pixel using alpha blending.
    /// </summary>
    /// <remarks>dstRGB = (srcRGB * srcA) + (dstRGB * (1 - srcA)), dstA = srcA + (dstA * (1 - srcA))</remarks>
    Alpha = 0x00000001U,

    /// <summary>
    /// Blends the source pixel with the destination pixel using additive blending.
    /// </summary>
    /// <remarks>dstRGBA = srcRGBA + (dstRGBA * (1 - srcA))</remarks>
    PremultipliedAlpha = 0x00000010U,

    /// <summary>
    /// Blends the source pixel with the destination pixel using additive blending.
    /// </summary>
    /// <remarks>dstRGB = (srcRGB * srcA) + dstRGB, dstA = dstA</remarks>
    Additive = 0x00000002U,

    /// <summary>
    /// Blends the source pixel with the destination pixel using a premultiplied additive blending.
    /// </summary>
    /// <remarks>dstRGB = srcRGB + dstRGB, dstA = dstA</remarks>
    PremultipliedAdditive = 0x00000020U,

    /// <summary>
    /// Blends the source pixel with the destination pixel using color modulation.
    /// </summary>
    /// <remarks>dstRGB = srcRGB * dstRGB, dstA = dstA</remarks >
    ColorModulate = 0x00000004U,

    /// <summary>
    /// Blends the source pixel with the destination pixel using color multiplication.
    /// </summary>
    /// <remarks>dstRGB = (srcRGB * dstRGB) + (dstRGB * (1 - srcA)), dstA = dstA</remarks>
    ColorMultiply = 0x00000008U,

    /// <summary>
    /// The blend mode is invalid.
    /// </summary>
    Invalid = 0x7FFFFFFFU,
}
