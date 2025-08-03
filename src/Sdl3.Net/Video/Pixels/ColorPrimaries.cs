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
/// Represents the color primaries used in pixel formats.
/// </summary>
public enum ColorPrimaries
{
    /// <summary>
    /// Unknown color primaries.
    /// </summary>
    Unknown,

    /// <summary>
    /// ITU-R BT.709-6 color primaries, used in HDTV and some digital cinema formats.
    /// </summary>
    Bt709,

    /// <summary>
    /// Unspecified color primaries, used when the color primaries are not defined.
    /// </summary>
    Unspecified,

    /// <summary>
    /// ITU-R BT.470-6 System M color primaries, used in NTSC analog television.
    /// </summary>
    Bt470M = 4,

    /// <summary>
    /// ITU-R BT.470-6 system B, G / ITU-R BT.601-7 625 line system color primaries, used in PAL and SECAM analog television.
    /// </summary>
    Bt470Bg,

    /// <summary>
    /// ITU-R BT.601-7 525, SMPTE 170M color primaries, used in NTSC analog television and some digital formats.
    /// </summary>
    Bt601,

    /// <summary>
    /// SMPTE 240M, functionally the same as <see cref="Bt601"/> but with different chromaticity coordinates,
    /// used in some digital cinema formats.
    /// </summary>
    Smpte240,

    /// <summary>
    /// Generic film (color filters using Illuminant C), used in some film and digital cinema formats.
    /// </summary>
    GenericFilm,

    /// <summary>
    /// ITU-R BT.2020-2 / ITU-R BT.2100-0 color primaries, used in UHDTV and some digital cinema formats.
    /// </summary>
    Bt2020,

    /// <summary>
    /// SMPTE ST 428-1 color primaries, used in digital cinema formats.
    /// </summary>
    Xyz,

    /// <summary>
    /// SMPTE RP 431-2 color primaries, used in digital cinema formats.
    /// </summary>
    Smpte431,

    /// <summary>
    /// SMPTE EG 432-1 / DCI P3 color primaries, used in digital cinema formats.
    /// </summary>
    Smpte432,

    /// <summary>
    /// EBU Tech. 3213-E color primaries, used in some broadcast and digital cinema formats.
    /// </summary>
    Ebu3213 = 22,
}
