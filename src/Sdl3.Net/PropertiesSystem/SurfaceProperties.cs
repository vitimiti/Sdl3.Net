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

using Sdl3.Net.Shapes;
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.PropertiesSystem;

/// <summary>
/// Represents surface properties in the SDL properties system.
/// </summary>
public sealed class SurfaceProperties : Properties
{
    /// <summary>
    /// Gets the SDR white point of the surface.
    /// </summary>
    public float SdrWhitePoint => GetFloatProperty(SDL_PROP_SURFACE_SDR_WHITE_POINT_FLOAT);

    /// <summary>
    /// Gets the HDR headroom of the surface.
    /// </summary>
    public float HdrHeadroom => GetFloatProperty(SDL_PROP_SURFACE_HDR_HEADROOM_FLOAT);

    /// <summary>
    /// Gets the tonemap operator of the surface.
    /// </summary>
    public string? TonemapOperator => GetStringProperty(SDL_PROP_SURFACE_TONEMAP_OPERATOR_STRING);

    /// <summary>
    /// Gets the hotspot of the surface.
    /// </summary>
    public Point Hotspot =>
        new(
            (int)GetNumberProperty(SDL_PROP_SURFACE_HOTSPOT_X_NUMBER),
            (int)GetNumberProperty(SDL_PROP_SURFACE_HOTSPOT_Y_NUMBER)
        );

    internal SurfaceProperties(SDL_PropertiesID propertiesId)
        : base(propertiesId) { }
}
