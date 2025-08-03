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
/// Represents the layout of packed pixel formats in SDL.
/// </summary>
public enum PackedLayout
{
    /// <summary>
    /// Unknown packed pixel format layout.
    /// </summary>
    None,

    /// <summary>
    /// Packed pixel format with 3 bits for red, 3 bits for green, and 2 bits for blue channels.
    /// </summary>
    Layout332,

    /// <summary>
    /// Packed pixel format with 4 bits for red, 4 bits for green, 4 bits for blue, and 4 bits for alpha channels.
    /// </summary>
    Layout4444,

    /// <summary>
    /// Packed pixel format with 1 bit for alpha channel, 5 bits for red, 5 bits for green, and 5 bits for blue channels.
    /// </summary>
    Layout1555,

    /// <summary>
    /// Packed pixel format with 5 bits for red, 5 bits for green, and 5 bits for blue channels, and 1 bit for alpha channel.
    /// </summary>
    Layout5551,

    /// <summary>
    /// Packed pixel format with 5 bits for red, 6 bits for green, and 5 bits for blue channels.
    /// </summary>
    Layout565,

    /// <summary>
    /// Packed pixel format with 8 bits for red, 8 bits for green, and 8 bits for blue channels, and 8 bits for alpha channel.
    /// </summary>
    Layout8888,

    /// <summary>
    /// Packed pixel format with 2 bits for alpha, 10 bits for red, 10 bits for green, and 10 bits for blue channels.
    /// </summary>
    Layout2101010,

    /// <summary>
    /// Packed pixel format with 10 bits for red, 10 bits for green, 10 bits for blue, and 2 bits for alpha channels.
    /// </summary>
    Layout1010102,
}
