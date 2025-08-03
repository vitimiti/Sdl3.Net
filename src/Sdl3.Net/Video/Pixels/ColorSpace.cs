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
/// Represents a color space used in pixel formats.
/// </summary>
public class ColorSpace
{
    private readonly SDL_Colorspace _colorspace;

    /// <summary>
    /// Gets the color type of the color space.
    /// </summary>
    public ColorType Type => (ColorType)((uint)_colorspace >> 28 & 0x0F);

    /// <summary>
    /// Gets the color range of the color space.
    /// </summary>
    public ColorRange Range => (ColorRange)((uint)_colorspace >> 24 & 0x0F);

    /// <summary>
    /// Gets the chroma location of the color space.
    /// </summary>
    public ChromaLocation Chroma => (ChromaLocation)((uint)_colorspace >> 20 & 0x0F);

    /// <summary>
    /// Gets the color primaries of the color space.
    /// </summary>
    public ColorPrimaries Primaries => (ColorPrimaries)((uint)_colorspace >> 10 & 0x1F);

    /// <summary>
    /// Gets the transfer characteristics of the color space.
    /// </summary>
    public TransferCharacteristics Transfer =>
        (TransferCharacteristics)((uint)_colorspace >> 5 & 0x1F);

    /// <summary>
    /// Gets the matrix coefficients of the color space.
    /// </summary>
    public MatrixCoefficients Matrix => (MatrixCoefficients)((uint)_colorspace & 0x1F);

    /// <summary>
    /// Checks if the color space is BT.601.
    /// </summary>
    public bool IsMatrixBt601 => Matrix is MatrixCoefficients.Bt601 or MatrixCoefficients.Bt470Bg;

    /// <summary>
    /// Checks if the color space is BT.709.
    /// </summary>
    public bool IsMatrixBt709 => Matrix is MatrixCoefficients.Bt709;

    /// <summary>
    /// Checks if the color space is BT.2020 NCL.
    /// </summary>
    public bool IsBt2020Ncl => Matrix is MatrixCoefficients.Bt2020Ncl;

    /// <summary>
    /// Checks if the color range is limited.
    /// </summary>
    public bool IsLimitedRange => Range is not ColorRange.Full;

    /// <summary>
    /// Checks if the color space is full range.
    /// </summary>
    public bool IsFullRange => Range is ColorRange.Full;

    private ColorSpace(uint colorspace) => _colorspace = (SDL_Colorspace)colorspace;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with an unknown color space.
    /// </summary>
    public static ColorSpace Unknown => new(0);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with an SRGB color space.
    /// </summary>
    public static ColorSpace Srgb => new(0x120005A0U);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with an SRGB linear color space.
    /// </summary>
    public static ColorSpace SrgbLinear => new(0x12000500U);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with an HDR10 color space.
    /// </summary>
    public static ColorSpace Hdr10 => new(0x12002600U);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with a JPEG color space.
    /// </summary>
    public static ColorSpace Jpeg => new(0x220004C6U);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with a BT.601 limited color space.
    /// </summary>
    public static ColorSpace Bt601Limited => new(0x211018C6U);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with a BT.601 full color space.
    /// </summary>
    public static ColorSpace Bt601Full => new(0x221018C6U);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with a BT.709 limited color space.
    /// </summary>
    public static ColorSpace Bt709Limited => new(0x21100421U);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with a BT.709 full color space.
    /// </summary>
    public static ColorSpace Bt709Full => new(0x22100421U);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with a BT.2020 limited color space.
    /// </summary>
    public static ColorSpace Bt2020Limited => new(0x21102609U);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with a BT.2020 full color space.
    /// </summary>
    public static ColorSpace Bt2020Full => new(0x22102609U);

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with a RGB default color space.
    /// </summary>
    public static ColorSpace RgbDefault => Srgb;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorSpace"/> class with a YUV default color space.
    /// </summary>
    public static ColorSpace YuvDefault => Jpeg;

    /// <summary>
    /// Gets the information about the color space as a string.
    /// </summary>
    public override string ToString()
    {
        return $"{nameof(ColorSpace)}({nameof(Type)}: {Type}, {nameof(Range)}: {Range}, {nameof(Chroma)}: {Chroma}, {nameof(Primaries)}: {Primaries}, {nameof(Transfer)}: {Transfer}, {nameof(Matrix)}: {Matrix})";
    }
}
