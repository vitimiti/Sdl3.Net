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
/// Represents the transfer characteristics of pixel formats in SDL.
/// </summary>
public enum TransferCharacteristics
{
    /// <summary>
    /// Unknown transfer characteristics.
    /// </summary>
    Unknown,

    /// <summary>
    /// Rec. ITU-R BT.709-6 / ITU-R BT1361 transfer characteristics.
    /// </summary>
    Bt709,

    /// <summary>
    /// Unspecified transfer characteristics.
    /// </summary>
    Unspecified,

    /// <summary>
    /// ITU-R BT.470-6 System M / ITU-R BT1700 625 PAL <![CDATA[&]]> SECAM transfer characteristics.
    /// </summary>
    Gamma22 = 4,

    /// <summary>
    /// ITU-R BT.470-6 System B, G transfer characteristics.
    /// </summary>
    Gamma28,

    /// <summary>
    /// SMPTE ST 170M / ITU-R BT.601-7 525 or 625 transfer characteristics.
    /// </summary>
    Bt601,

    /// <summary>
    /// SMPTE ST 240M transfer characteristics.
    /// </summary>
    Smpte240,

    /// <summary>
    /// Linear transfer characteristics.
    /// </summary>
    Linear,

    /// <summary>
    /// Logarithmic transfer characteristics with base 10.
    /// </summary>
    Log100,

    /// <summary>
    /// Logarithmic transfer characteristics with base 10 and square root.
    /// </summary>
    Log100Sqrt10,

    /// <summary>
    /// IEC 61966-2-4 transfer characteristics.
    /// </summary>
    Iec61966,

    /// <summary>
    /// ITU-R BT1361 Extended Colour Gamut transfer characteristics.
    /// </summary>
    Bt1361,

    /// <summary>
    /// IEC 61966-2-1 (sRGB or sYCC) transfer characteristics.
    /// </summary>
    Srgb,

    /// <summary>
    /// ITU-R BT2020 for 10-bit systems transfer characteristics.
    /// </summary>
    Bt2020With10Bit,

    /// <summary>
    /// ITU-R BT2020 for 12-bit systems transfer characteristics.
    /// </summary>
    Bt2020With12Bit,

    /// <summary>
    /// SMPTE ST 2084 for 10-, 12-, 14- and 16-bit systems transfer characteristics, also known as PQ (Perceptual Quantizer).
    /// </summary>
    Pq,

    /// <summary>
    /// SMPTE ST 428-1 transfer characteristics.
    /// </summary>
    Smpte428,

    /// <summary>
    /// ARIB STD-B67, known as "Hybrid Log-Gamma" (HLG), transfer characteristics.
    /// </summary>
    Hlg,
}
