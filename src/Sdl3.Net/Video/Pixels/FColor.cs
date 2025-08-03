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
/// Represents a floating-point color with red, green, blue, and alpha components.
/// </summary>
/// <param name="Red">The red component of the color.</param>
/// <param name="Green">The green component of the color.</param>
/// <param name="Blue">The blue component of the color.</param>
/// <param name="Alpha">The alpha component of the color.</param>
public record FColor(float Red, float Green, float Blue, float Alpha = 1F)
{
    /// <summary>
    /// Gets the red component of the color, clamped between 0.0 and 1.0.
    /// </summary>
    public float Red { get; init; } = float.Clamp(Red, 0F, 1F);

    /// <summary>
    /// Gets the green component of the color, clamped between 0.0 and 1.0.
    /// </summary>
    public float Green { get; init; } = float.Clamp(Green, 0F, 1F);

    /// <summary>
    /// Gets the blue component of the color, clamped between 0.0 and 1.0.
    /// </summary>
    public float Blue { get; init; } = float.Clamp(Blue, 0F, 1F);

    /// <summary>
    /// Gets the alpha component of the color, clamped between 0.0 and 1.0.
    /// </summary>
    public float Alpha { get; init; } = float.Clamp(Alpha, 0F, 1F);

    /// <summary>
    /// Converts this floating-point color to a standard <see cref="Color"/> with byte components
    /// by scaling the floating-point values to the range [0, 255].
    /// </summary>
    public Color ToColor() =>
        new((byte)(Red * 255), (byte)(Green * 255), (byte)(Blue * 255), (byte)(Alpha * 255));
}
