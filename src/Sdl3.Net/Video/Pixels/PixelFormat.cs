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

using System.Runtime.InteropServices;
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.Video.Pixels;

/// <summary>
/// Represents a pixel format in SDL.
/// </summary>
public class PixelFormat
{
    private readonly SDL_PixelFormat _format;

    /// <summary>
    /// Gets the flags of the pixel format.
    /// </summary>
    public uint Flags => (uint)_format >> 28 & 0x0F;

    /// <summary>
    /// Gets the type of the pixel format.
    /// </summary>
    public PixelType Type => (PixelType)((uint)_format >> 24 & 0x0F);

    /// <summary>
    /// Gets the array order of the pixel format.
    /// </summary>
    public ArrayOrder ArrayOrder =>
        IsArray ? (ArrayOrder)((uint)_format >> 20 & 0x0F) : ArrayOrder.None;

    /// <summary>
    /// Gets the bitmap order of the pixel format.
    /// </summary>
    public BitmapOrder BitmapOrder =>
        !IsArray && !IsPacked ? (BitmapOrder)((uint)_format >> 20 & 0x0F) : BitmapOrder.None;

    /// <summary>
    /// Gets the packed order of the pixel format.
    /// </summary>
    public PackedOrder PackedOrder =>
        IsPacked ? (PackedOrder)((uint)_format >> 20 & 0x0F) : PackedOrder.None;

    /// <summary>
    /// Gets the packed layout of the pixel format.
    /// </summary>
    public PackedLayout Layout => (PackedLayout)((uint)_format >> 16 & 0x0F);

    /// <summary>
    /// Gets the bits per pixel of the pixel format.
    /// </summary>
    public uint BitsPerPixel => IsFourCc ? 0 : (uint)_format >> 8 & 0xFF;

    /// <summary>
    /// Gets the bytes per pixel of the pixel format.
    /// </summary>
    public uint BytesPerPixel =>
        IsFourCc
            ? (
                _format == SDL_PIXELFORMAT_YUY2
                || _format == SDL_PIXELFORMAT_UYVY
                || _format == SDL_PIXELFORMAT_YVYU
                || _format == SDL_PIXELFORMAT_P010
                    ? 2U
                    : 1U
            )
            : (uint)_format & 0xFF;

    /// <summary>
    /// Gets whether the pixel format is indexed.
    /// </summary>
    public bool IsIndexed =>
        !IsFourCc
        && Type is PixelType.Index1 or PixelType.Index2 or PixelType.Index4 or PixelType.Index8;

    /// <summary>
    /// Gets whether the pixel format is packed.
    /// </summary>
    public bool IsPacked =>
        !IsFourCc && Type is PixelType.Packed8 or PixelType.Packed16 or PixelType.Packed32;

    /// <summary>
    /// Gets whether the pixel format is an array.
    /// </summary>
    public bool IsArray =>
        !IsFourCc
        && Type
            is PixelType.ArrayU8
                or PixelType.ArrayU16
                or PixelType.ArrayU32
                or PixelType.ArrayF16
                or PixelType.ArrayF32;

    /// <summary>
    /// Gets whether the pixel format is 10 bits.
    /// </summary>
    public bool Is10Bit =>
        !IsFourCc && Type is PixelType.Packed32 && Layout is PackedLayout.Layout2101010;

    /// <summary>
    /// Gets whether the pixel format is a floating pixel.
    /// </summary>
    public bool IsFloat => !IsFourCc && Type is PixelType.ArrayF16 or PixelType.ArrayF32;

    /// <summary>
    /// Gets whether the pixel has an alpha channel.
    /// </summary>
    public bool IsAlpha =>
        (
            IsPacked
            && PackedOrder
                is PackedOrder.Argb
                    or PackedOrder.Rgba
                    or PackedOrder.Abgr
                    or PackedOrder.Bgra
        )
        || (
            IsArray
            && ArrayOrder
                is ArrayOrder.Argb
                    or ArrayOrder.Rgba
                    or ArrayOrder.Abgr
                    or ArrayOrder.Bgra
        );

    /// <summary>
    /// Gets whether the pixel format is a FourCC format.
    /// </summary>
    public bool IsFourCc => _format != 0 && Flags != 1;

    private PixelFormat(
        PixelType type,
        uint order,
        PackedLayout layout,
        uint bits,
        uint bitsPerPixel
    ) =>
        _format = (SDL_PixelFormat)(
            1 << 28 | (uint)type << 24 | order << 20 | (uint)layout << 16 | bits << 8 | bitsPerPixel
        );

    internal PixelFormat(SDL_PixelFormat format) => _format = format;

    /// <summary>
    /// Creates a new instance of the <see cref="PixelFormat"/> class.
    /// </summary>
    /// <param name="type">The type of the pixel format.</param>
    /// <param name="order">The array order of the pixel format.</param>
    /// <param name="layout">The layout of the pixel format.</param>
    /// <param name="bits">The bits of the pixel format.</param>
    /// <param name="bitsPerPixel">The bits per pixel of the pixel format.</param>
    public PixelFormat(
        PixelType type,
        ArrayOrder order,
        PackedLayout layout,
        uint bits,
        uint bitsPerPixel
    )
        : this(type, (uint)order, layout, bits, bitsPerPixel) { }

    /// <summary>
    /// Creates a new instance of the <see cref="PixelFormat"/> class.
    /// </summary>
    /// <param name="type">The type of the pixel format.</param>
    /// <param name="order">The bitmap order of the pixel format.</param>
    /// <param name="layout">The layout of the pixel format.</param>
    /// <param name="bits">The bits of the pixel format.</param>
    /// <param name="bitsPerPixel"></param>
    public PixelFormat(
        PixelType type,
        BitmapOrder order,
        PackedLayout layout,
        uint bits,
        uint bitsPerPixel
    )
        : this(type, (uint)order, layout, bits, bitsPerPixel) { }

    /// <summary>
    /// Creates a new instance of the <see cref="PixelFormat"/> class.
    /// </summary>
    /// <param name="type">The type of the pixel format.</param>
    /// <param name="order">The packed order of the pixel format.</param>
    /// <param name="layout">The layout of the pixel format.</param>
    /// <param name="bits">The bits of the pixel format.</param>
    /// <param name="bitsPerPixel">The bits per pixel of the pixel format.</param>
    public PixelFormat(
        PixelType type,
        PackedOrder order,
        PackedLayout layout,
        uint bits,
        uint bitsPerPixel
    )
        : this(type, (uint)order, layout, bits, bitsPerPixel) { }

    /// <summary>
    /// Creates a new instance of the <see cref="PixelFormat"/> class.
    /// </summary>
    /// <param name="type">The type of the pixel format.</param>
    /// <param name="layout">The layout of the pixel format.</param>
    /// <param name="bits">The bits of the pixel format.</param>
    /// <param name="bitsPerPixel">The bits per pixel of the pixel format.</param>
    public PixelFormat(PixelType type, PackedLayout layout, uint bits, uint bitsPerPixel)
        : this(type, 0U, layout, bits, bitsPerPixel) { }

    /// <summary>
    /// Creates a new instance of the <see cref="PixelFormat"/> class.
    /// </summary>
    /// <param name="a">The red component of the pixel format.</param>
    /// <param name="b">The green component of the pixel format.</param>
    /// <param name="c">The blue component of the pixel format.</param>
    /// <param name="d">The alpha component of the pixel format.</param>
    public PixelFormat(byte a, byte b, byte c, byte d) =>
        _format = (SDL_PixelFormat)((uint)a << 0 | (uint)b << 8 | (uint)c << 16 | (uint)d << 24);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an unknown pixel format.
    /// </summary>
    public static PixelFormat Unknown => new(SDL_PIXELFORMAT_UNKNOWN);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a 1-bit indexed pixel format with
    /// least significant bit first.
    /// </summary>
    public static PixelFormat Index1Lsb => new(SDL_PIXELFORMAT_INDEX1LSB);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a 1-bit indexed pixel format with
    /// most significant bit first.
    /// </summary>
    public static PixelFormat Index1Msb => new(SDL_PIXELFORMAT_INDEX1MSB);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a 2-bit indexed pixel format with
    /// least significant bit first.
    /// </summary>
    public static PixelFormat Index2Lsb => new(SDL_PIXELFORMAT_INDEX2LSB);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a 2-bit indexed pixel format with
    /// most significant bit first.
    /// </summary>
    public static PixelFormat Index2Msb => new(SDL_PIXELFORMAT_INDEX2MSB);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a 4-bit indexed pixel format with
    /// least significant bit first.
    /// </summary>
    public static PixelFormat Index4Lsb => new(SDL_PIXELFORMAT_INDEX4LSB);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a 4-bit indexed pixel format with
    /// most significant bit first.
    /// </summary>
    public static PixelFormat Index4Msb => new(SDL_PIXELFORMAT_INDEX4MSB);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an 8-bit indexed pixel format.
    /// </summary>
    public static PixelFormat Index8 => new(SDL_PIXELFORMAT_INDEX8);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGB332 packed pixel format.
    /// </summary>
    public static PixelFormat Rgb332 => new(SDL_PIXELFORMAT_RGB332);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an XRGB4444 packed pixel format.
    /// </summary>
    public static PixelFormat Xrgb4444 => new(SDL_PIXELFORMAT_XRGB4444);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an XBGR4444 packed pixel format.
    /// </summary>
    public static PixelFormat Xbgr4444 => new(SDL_PIXELFORMAT_XBGR4444);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an XRGB1555 packed pixel format.
    /// </summary>
    public static PixelFormat Xrgb1555 => new(SDL_PIXELFORMAT_XRGB1555);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an XBGR1555 packed pixel format.
    /// </summary>
    public static PixelFormat Xbgr1555 => new(SDL_PIXELFORMAT_XBGR1555);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ARGB4444 packed pixel format.
    /// </summary>
    public static PixelFormat Argb4444 => new(SDL_PIXELFORMAT_ARGB4444);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGBA4444 packed pixel format.
    /// </summary>
    public static PixelFormat Rgba4444 => new(SDL_PIXELFORMAT_RGBA4444);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ABGR4444 packed pixel format.
    /// </summary>
    public static PixelFormat Abgr4444 => new(SDL_PIXELFORMAT_ABGR4444);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGRA4444 packed pixel format.
    /// </summary>
    public static PixelFormat Bgra4444 => new(SDL_PIXELFORMAT_BGRA4444);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ARGB1555 packed pixel format.
    /// </summary>
    public static PixelFormat Argb1555 => new(SDL_PIXELFORMAT_ARGB1555);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGBA5551 packed pixel format.
    /// </summary>
    public static PixelFormat Rgba5551 => new(SDL_PIXELFORMAT_RGBA5551);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ABGR1555 packed pixel format.
    /// </summary>
    public static PixelFormat Abgr1555 => new(SDL_PIXELFORMAT_ABGR1555);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGRA5551 packed pixel format.
    /// </summary>
    public static PixelFormat Bgra5551 => new(SDL_PIXELFORMAT_BGRA5551);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGB565 packed pixel format.
    /// </summary>
    public static PixelFormat Rgb565 => new(SDL_PIXELFORMAT_RGB565);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGR565 packed pixel format.
    /// </summary>
    public static PixelFormat Bgr565 => new(SDL_PIXELFORMAT_BGR565);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGB24 packed pixel format.
    /// </summary>
    public static PixelFormat Rgb24 => new(SDL_PIXELFORMAT_RGB24);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGR24 packed pixel format.
    /// </summary>
    public static PixelFormat Bgr24 => new(SDL_PIXELFORMAT_BGR24);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an XRGB8888 packed pixel format.
    /// </summary>
    public static PixelFormat Xrgb8888 => new(SDL_PIXELFORMAT_XRGB8888);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGBX8888 packed pixel format.
    /// </summary>
    public static PixelFormat Rgbx8888 => new(SDL_PIXELFORMAT_RGBX8888);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an XBGR8888 packed pixel format.
    /// </summary>
    public static PixelFormat Xbgr8888 => new(SDL_PIXELFORMAT_XBGR8888);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGRX8888 packed pixel format.
    /// </summary>
    public static PixelFormat Bgrx8888 => new(SDL_PIXELFORMAT_BGRX8888);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ARGB8888 packed pixel format.
    /// </summary>
    public static PixelFormat Argb8888 => new(SDL_PIXELFORMAT_ARGB8888);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGBA8888 packed pixel format.
    /// </summary>
    public static PixelFormat Rgba8888 => new(SDL_PIXELFORMAT_RGBA8888);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ABGR8888 packed pixel format.
    /// </summary>
    public static PixelFormat Abgr8888 => new(SDL_PIXELFORMAT_ABGR8888);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGRA8888 packed pixel format.
    /// </summary>
    public static PixelFormat Bgra8888 => new(SDL_PIXELFORMAT_BGRA8888);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an XRGB2101010 packed pixel format.
    /// </summary>
    public static PixelFormat Xxrgb2101010 => new(SDL_PIXELFORMAT_XRGB2101010);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an XBGR2101010 packed pixel format.
    /// </summary>
    public static PixelFormat Xxbgr2101010 => new(SDL_PIXELFORMAT_XBGR2101010);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ARGB2101010 packed pixel format.
    /// </summary>
    public static PixelFormat Argb2101010 => new(SDL_PIXELFORMAT_ARGB2101010);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ABGR2101010 packed pixel format.
    /// </summary>
    public static PixelFormat Abgr2101010 => new(SDL_PIXELFORMAT_ABGR2101010);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGB48 packed pixel format.
    /// </summary>
    public static PixelFormat Rgb48 => new(SDL_PIXELFORMAT_RGB48);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGR48 packed pixel format.
    /// </summary>
    public static PixelFormat Bgr48 => new(SDL_PIXELFORMAT_BGR48);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGBA64 packed pixel format.
    /// </summary>
    public static PixelFormat Rgba64 => new(SDL_PIXELFORMAT_RGBA64);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ARGB64 packed pixel format.
    /// </summary>
    public static PixelFormat Argb64 => new(SDL_PIXELFORMAT_ARGB64);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGRA64 packed pixel format.
    /// </summary>
    public static PixelFormat Bgra64 => new(SDL_PIXELFORMAT_BGRA64);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ABGR64 packed pixel format.
    /// </summary>
    public static PixelFormat Abgr64 => new(SDL_PIXELFORMAT_ABGR64);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGB48 floating pixel format.
    /// </summary>
    public static PixelFormat Rgb48Float => new(SDL_PIXELFORMAT_RGB48_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGR48 floating pixel format.
    /// </summary>
    public static PixelFormat Bgr48Float => new(SDL_PIXELFORMAT_BGR48_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGBA64 floating pixel format.
    /// </summary>
    public static PixelFormat Rgba64Float => new(SDL_PIXELFORMAT_RGBA64_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ARGB64 floating pixel format.
    /// </summary>
    public static PixelFormat Argb64Float => new(SDL_PIXELFORMAT_ARGB64_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGRA64 floating pixel format.
    /// </summary>
    public static PixelFormat Bgra64Float => new(SDL_PIXELFORMAT_BGRA64_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ABGR64 floating pixel format.
    /// </summary>
    public static PixelFormat Abgr64Float => new(SDL_PIXELFORMAT_ABGR64_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGB96 floating pixel format.
    /// </summary>
    public static PixelFormat Rgb96Float => new(SDL_PIXELFORMAT_RGB96_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGR96 floating pixel format.
    /// </summary>
    public static PixelFormat Bgr96Float => new(SDL_PIXELFORMAT_BGR96_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an RGBA128 floating pixel format.
    /// </summary>
    public static PixelFormat Rgba128Float => new(SDL_PIXELFORMAT_RGBA128_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ARGB128 floating pixel format.
    /// </summary>
    public static PixelFormat Argb128Float => new(SDL_PIXELFORMAT_ARGB128_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGRA128 floating pixel format.
    /// </summary>
    public static PixelFormat Bgra128Float => new(SDL_PIXELFORMAT_BGRA128_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an ABGR128 floating pixel format.
    /// </summary>
    public static PixelFormat Abgr128Float => new(SDL_PIXELFORMAT_ABGR128_FLOAT);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a YUV packed pixel format.
    /// </summary>
    public static PixelFormat Yv12 => new(SDL_PIXELFORMAT_YV12);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an IYUV packed pixel format.
    /// </summary>
    public static PixelFormat Iyuv => new(SDL_PIXELFORMAT_IYUV);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an YUY2 packed pixel format.
    /// </summary>
    public static PixelFormat Yuy2 => new(SDL_PIXELFORMAT_YUY2);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an UYVY packed pixel format.
    /// </summary>
    public static PixelFormat Uyvy => new(SDL_PIXELFORMAT_UYVY);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a YVYU packed pixel format.
    /// </summary>
    public static PixelFormat Yvyu => new(SDL_PIXELFORMAT_YVYU);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an NV12 packed pixel format.
    /// </summary>
    public static PixelFormat Nv12 => new(SDL_PIXELFORMAT_NV12);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing an NV21 packed pixel format.
    /// </summary>
    public static PixelFormat Nv21 => new(SDL_PIXELFORMAT_NV21);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a P010 packed pixel format.
    /// </summary>
    public static PixelFormat P010 => new(SDL_PIXELFORMAT_P010);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a P012 packed pixel format.
    /// </summary>
    public static PixelFormat ExternalOes => new(SDL_PIXELFORMAT_EXTERNAL_OES);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a MJPEG packed pixel format.
    /// </summary>
    public static PixelFormat Mjpg => new(SDL_PIXELFORMAT_MJPG);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a RGBA32 pixel format.
    /// </summary>
    public static PixelFormat Rgba32 => new(SDL_PIXELFORMAT_RGBA32);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a ARGB32 pixel format.
    /// </summary>
    public static PixelFormat Argb32 => new(SDL_PIXELFORMAT_ARGB32);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGRA32 pixel format.
    /// </summary>
    public static PixelFormat Bgra32 => new(SDL_PIXELFORMAT_BGRA32);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a ABGR32 pixel format.
    /// </summary>
    public static PixelFormat Abgr32 => new(SDL_PIXELFORMAT_ABGR32);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a RGBX32 pixel format.
    /// </summary>
    public static PixelFormat Rgbx32 => new(SDL_PIXELFORMAT_RGBX32);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a XRGB32 pixel format.
    /// </summary>
    public static PixelFormat Xrgb32 => new(SDL_PIXELFORMAT_XRGB32);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a BGRX32 pixel format.
    /// </summary>
    public static PixelFormat Bgrx32 => new(SDL_PIXELFORMAT_BGRX32);

    /// <summary>
    /// Gets a <see cref="PixelFormat"/> instance representing a XBGR32 pixel format.
    /// </summary>
    public static PixelFormat Xbgr32 => new(SDL_PIXELFORMAT_XBGR32);

    /// <summary>
    /// Gets the masks for the pixel format.
    /// </summary>
    /// <param name="bitsPerPixel">The bits per pixel.</param>
    /// <returns>The masks for the pixel format.</returns>
    public PixelFormatMasks GetMasks(out int bitsPerPixel)
    {
        return !SDL_GetMasksForPixelFormat(
            _format,
            out bitsPerPixel,
            out var rMask,
            out var gMask,
            out var bMask,
            out var aMask
        )
            ? throw new ExternalException($"Failed to get masks for pixel format: {SDL_GetError()}")
            : new PixelFormatMasks(rMask, gMask, bMask, aMask);
    }

    /// <summary>
    /// Gets the details of the pixel format.
    /// </summary>
    public PixelFormatDetails GetDetails()
    {
        unsafe
        {
            var details = SDL_GetPixelFormatDetails(_format);
            return details is null
                ? throw new ExternalException(
                    $"Failed to get details for pixel format: {SDL_GetError()}"
                )
                : new PixelFormatDetails(details);
        }
    }

    /// <summary>
    /// Gets the name of the pixel format.
    /// </summary>
    public override string ToString() => SDL_GetPixelFormatName(_format);
}
