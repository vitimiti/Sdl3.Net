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

using static Sdl3.Net.Imports.SDL3;

/// <summary>
/// Represents the masks for pixel format components.
/// </summary>
/// <param name="Red">The mask for the red component.</param>
/// <param name="Green">The mask for the green component.</param>
/// <param name="Blue">The mask for the blue component.</param>
/// <param name="Alpha">The mask for the alpha component.</param>
public record PixelFormatMasks(uint Red, uint Green, uint Blue, uint Alpha)
{
    /// <summary>
    /// Gets the pixel format for the specified masks.
    /// </summary>
    /// <param name="bitsPerPixel">The bits per pixel.</param>
    /// <returns>The pixel format for the specified masks.</returns>
    public PixelFormat GetPixelFormat(int bitsPerPixel) =>
        new(SDL_GetPixelFormatForMasks(bitsPerPixel, Red, Green, Blue, Alpha));
}
