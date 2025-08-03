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

using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.Video.Pixels;

/// <summary>
/// Represents the details of a pixel format.
/// </summary>
public class PixelFormatDetails
{
    internal unsafe SDL_PixelFormatDetails* Handle { get; }

    /// <summary>
    /// Gets the pixel format these details represent.
    /// </summary>
    public PixelFormat Format
    {
        get
        {
            unsafe
            {
                return new PixelFormat(Handle->format);
            }
        }
    }

    /// <summary>
    /// Gets the bits per pixel of the pixel format.
    /// </summary>
    public byte BitsPerPixel
    {
        get
        {
            unsafe
            {
                return Handle->bits_per_pixel;
            }
        }
    }

    /// <summary>
    /// Gets the bytes per pixel of the pixel format.
    /// </summary>
    public byte BytesPerPixel
    {
        get
        {
            unsafe
            {
                return Handle->bytes_per_pixel;
            }
        }
    }

    /// <summary>
    /// Gets the masks for the pixel format.
    /// </summary>
    public PixelFormatMasks Masks
    {
        get
        {
            unsafe
            {
                return new PixelFormatMasks(
                    Handle->Rmask,
                    Handle->Gmask,
                    Handle->Bmask,
                    Handle->Amask
                );
            }
        }
    }

    /// <summary>
    /// Gets the shifts for the pixel format.
    /// </summary>
    public PixelFormatBits Bits
    {
        get
        {
            unsafe
            {
                return new PixelFormatBits(
                    Handle->Rshift,
                    Handle->Gshift,
                    Handle->Bshift,
                    Handle->Ashift
                );
            }
        }
    }

    /// <summary>
    /// Gets the shifts for the pixel format.
    /// </summary>
    public PixelFormatShift Shift
    {
        get
        {
            unsafe
            {
                return new PixelFormatShift(
                    Handle->Rshift,
                    Handle->Gshift,
                    Handle->Bshift,
                    Handle->Ashift
                );
            }
        }
    }

    internal unsafe PixelFormatDetails(SDL_PixelFormatDetails* details) => Handle = details;

    /// <summary>
    /// Maps a color to a pixel value using the specified palette.
    /// </summary>
    /// <param name="color">The color to map.</param>
    /// <param name="palette">The palette to use for mapping if any.</param>
    /// <returns>The mapped pixel value.</returns>
    /// <remarks>
    /// This function only uses the RGB components of the color.
    /// </remarks>
    public ColoredPixel MapRgb(Color color, Palette? palette = null)
    {
        unsafe
        {
            return new ColoredPixel(
                SDL_MapRGB(
                    Handle,
                    palette is null ? null : palette.Handle,
                    color.Red,
                    color.Green,
                    color.Blue
                )
            );
        }
    }

    /// <summary>
    /// Maps a color to a pixel value using the specified palette, including alpha.
    /// </summary>
    /// <param name="color">The color to map.</param>
    /// <param name="palette">The palette to use for mapping, if any.</param>
    /// <returns>The mapped pixel value.</returns>
    public ColoredPixel MapRgba(Color color, Palette? palette = null)
    {
        unsafe
        {
            return new ColoredPixel(
                SDL_MapRGBA(
                    Handle,
                    palette is null ? null : palette.Handle,
                    color.Red,
                    color.Green,
                    color.Blue,
                    color.Alpha
                )
            );
        }
    }

    /// <summary>
    /// Returns a string representation of the pixel format details.
    /// </summary>
    public override string ToString() =>
        $"{nameof(PixelFormatDetails)}({nameof(Format)}: {Format}, {nameof(BitsPerPixel)}: {BitsPerPixel}, {nameof(BytesPerPixel)}: {BytesPerPixel}, {nameof(Masks)}: {Masks}, {nameof(Bits)}: {Bits}, {nameof(Shift)}: {Shift})";
}
