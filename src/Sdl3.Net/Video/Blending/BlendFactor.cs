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
/// Represents the blend factor used in custom blend modes.
/// </summary>
public enum BlendFactor
{
    /// <summary>
    /// The factor is zero, meaning no contribution from the source.
    /// </summary>
    /// <remarks>0, 0, 0, 0</remarks>
    Zero = 1,

    /// <summary>
    /// The factor is one, meaning full contribution from the source.
    /// </summary>
    /// <remarks>1, 1, 1, 1</remarks>
    One = 2,

    /// <summary>
    /// The factor is the source color, meaning the color of the source pixel is used.
    /// </summary>
    /// <remarks>srcR, srcG, srcB, srcA</remarks>
    SourceColor = 3,

    /// <summary>
    /// The factor is one minus the source color, meaning the inverse of the source pixel color is used.
    /// </summary>
    /// <remarks>1 - srcR, 1 - srcG, 1 - srcB, 1 - srcA</remarks>
    OneMinusSourceColor = 4,

    /// <summary>
    /// The factor is the source alpha, meaning the alpha value of the source pixel is used.
    /// </summary>
    /// <remarks>srcA, srcA, srcA, srcA</remarks>
    SourceAlpha = 5,

    /// <summary>
    /// The factor is one minus the source alpha, meaning the inverse of the source pixel alpha is used.
    /// </summary>
    /// <remarks>1 - srcA, 1 - srcA, 1 - srcA, 1 - srcA</remarks>
    OneMinusSourceAlpha = 6,

    /// <summary>
    /// The factor is the destination color, meaning the color of the destination pixel is used.
    /// </summary>
    /// <remarks>dstR, dstG, dstB, dstA</remarks>
    DestinationColor = 7,

    /// <summary>
    /// The factor is one minus the destination color, meaning the inverse of the destination pixel color is used.
    /// </summary>
    /// <remarks>1 - dstR, 1 - dstG, 1 - dstB, 1 - dstA</remarks>
    OneMinusDestinationColor = 8,

    /// <summary>
    /// The factor is the destination alpha, meaning the alpha value of the destination pixel is used.
    /// </summary>
    /// <remarks>dstA, dstA, dstA, dstA</remarks>
    DestinationAlpha = 9,

    /// <summary>
    /// The factor is one minus the destination alpha, meaning the inverse of the destination pixel alpha is used.
    /// </summary>
    /// <remarks>1 - dstA, 1 - dstA, 1 - dstA, 1 - dstA</remarks>
    OneMinusDestinationAlpha = 10,
}
