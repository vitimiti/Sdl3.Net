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

namespace Sdl3.Net.Video.Pixels;

/// <summary>
/// Represents the order of packed pixel formats in SDL.
/// </summary>
public enum PackedOrder
{
    /// <summary>
    /// Unknown packed pixel format order.
    /// </summary>
    None,

    /// <summary>
    /// Packed pixel format with unused alpha, red, green, and blue channels in that order.
    /// </summary>
    Xrgb,

    /// <summary>
    /// Packed pixel format with red, green, blue, and unused alpha channels in that order.
    /// </summary>
    Rgbx,

    /// <summary>
    /// Packed pixel format with alpha, red, green, blue channels in that order.
    /// </summary>
    Argb,

    /// <summary>
    /// Packed pixel format with red, green, blue, and alpha channels in that order.
    /// </summary>
    Rgba,

    /// <summary>
    /// Packed pixel format with unused alpha, blue, green, red channels in that order.
    /// </summary>
    Xbgr,

    /// <summary>
    /// Packed pixel format with blue, green, red, and unused alpha channels in that order.
    /// </summary>
    Bgrx,

    /// <summary>
    /// Packed pixel format with alpha, blue, green, red channels in that order.
    /// </summary>
    Abgr,

    /// <summary>
    /// Packed pixel format with alpha, blue, green, red channels in that order.
    /// </summary>
    Bgra,
}
