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
/// Represents the matrix coefficients used in pixel formats.
/// </summary>
public enum MatrixCoefficients
{
    /// <summary>
    /// Identity matrix coefficients.
    /// </summary>
    Identity,

    /// <summary>
    /// ITU-R BT.709-6 matrix coefficients.
    /// </summary>
    Bt709,

    /// <summary>
    /// Unspecified matrix coefficients.
    /// </summary>
    Unspecified,

    /// <summary>
    /// US FCC Title 47 Code of Federal Regulations Part 73.682 (a) (20) matrix coefficients.
    /// </summary>
    Fcc = 4,

    /// <summary>
    /// ITU-R BT.570-6 System B, G / ITU-R BT.601-7 625, functionally the same as <see cref="Bt601"/> matrix coefficients.
    /// </summary>
    Bt470Bg,

    /// <summary>
    /// ITU-R BT.601-7 525 matrix coefficients, used in NTSC and PAL analog television.
    /// </summary>
    Bt601,

    /// <summary>
    /// SMPTE 240M matrix coefficients.
    /// </summary>
    Smpte240,

    /// <summary>
    /// YCGC0 matrix coefficients, used in some digital cinema formats.
    /// </summary>
    Ycgc0,

    /// <summary>
    /// ITU-R BT.2020-2 non-constant luminance (NCL) matrix coefficients.
    /// </summary>
    Bt2020Ncl,

    /// <summary>
    /// ITU-R BT.2020-2 constant luminance (CL) matrix coefficients.
    /// </summary>
    Bt2020Cl,

    /// <summary>
    /// SMPTE ST 2085 matrix coefficients, used in some digital cinema formats.
    /// </summary>
    Smpte2085,

    /// <summary>
    /// Chroma derived non-constant luminance (NCL) matrix coefficients.
    /// </summary>
    ChromaDerivedNcl,

    /// <summary>
    /// Chroma derived constant luminance (CL) matrix coefficients.
    /// </summary>
    ChromaDerivedCl,

    /// <summary>
    /// ITU-R BT.2100-0 ICTCP matrix coefficients, used in UHDTV and some digital cinema formats.
    /// </summary>
    Ictcp,
}
