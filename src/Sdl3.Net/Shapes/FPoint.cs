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

using System.Runtime.InteropServices.Marshalling;
using Sdl3.Net.CustomMarshallers;

namespace Sdl3.Net.Shapes;

/// <summary>
/// Represents a floating-pointpoint in 2D space.
/// </summary>
/// <param name="X">The X coordinate of the point.</param>
/// <param name="Y">The Y coordinate of the point.</param>
[NativeMarshalling(typeof(FPointMarshaller))]
public record FPoint(float X, float Y)
{
    /// <summary>
    /// Gets a point at the origin (0, 0).
    /// </summary>
    public static FPoint Zero => new(0, 0);

    /// <summary>
    /// Gets a point on the X axis unit at (1, 0).
    /// </summary>
    public static FPoint UnitX => new(1, 0);

    /// <summary>
    /// Gets a point on the Y axis unit at (0, 1).
    /// </summary>
    public static FPoint UnitY => new(0, 1);

    /// <summary>
    /// Checks if this point is inside the specified rectangle.
    /// </summary>
    /// <param name="rect">The rectangle to check against.</param>
    /// <returns>True if the point is inside the rectangle, otherwise false.</returns>
    public bool IsInside(FRect rect) =>
        X >= rect.X && X < rect.X + rect.Width && Y >= rect.Y && Y < rect.Y + rect.Height;
}
