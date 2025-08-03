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
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.Shapes;

/// <summary>
/// Represents a floating-point rectangle in 2D space.
/// </summary>
/// <param name="X">The X coordinate of the rectangle.</param>
/// <param name="Y">The Y coordinate of the rectangle.</param>
/// <param name="Width">The width of the rectangle.</param>
/// <param name="Height">The height of the rectangle.</param>
[NativeMarshalling(typeof(FRectMarshaller))]
public record FRect(float X, float Y, float Width, float Height)
{
    /// <summary>
    /// Checks if this rectangle is empty (has zero width or height).
    /// </summary>
    public bool IsEmpty => Width <= 0 || Height <= 0;

    /// <summary>
    /// Checks if this rectangle intersects with another rectangle.
    /// </summary>
    /// <param name="other">The other rectangle to check for intersection.</param>
    /// <returns>True if the rectangles intersect, otherwise false.</returns>
    public bool Intersects(FRect other) => SDL_HasRectIntersectionFloat(this, other);

    /// <summary>
    /// Attempts to find the intersection of this rectangle with another rectangle.
    /// </summary>
    /// <param name="other">The other rectangle to check for intersection.</param>
    /// <param name="result">The intersection rectangle, if found.</param>
    /// <returns>True if the rectangles intersect, otherwise false.</returns>
    public bool TryIntersect(FRect other, out FRect? result)
    {
        if (SDL_GetRectIntersectionFloat(this, other, out var intersection))
        {
            result = intersection;
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Attempts to find the intersection of this rectangle with a line segment.
    /// </summary>
    /// <param name="line">The line segment defined by two points.</param>
    /// <param name="result">The intersection points, if found.</param>
    /// <returns>True if the line intersects the rectangle, otherwise false.</returns>
    public bool TryIntersect(
        (FPoint Start, FPoint End) line,
        out (FPoint Start, FPoint End)? result
    )
    {
        var x1 = line.Start.X;
        var y1 = line.Start.Y;
        var x2 = line.End.X;
        var y2 = line.End.Y;

        var valid = SDL_GetRectAndLineIntersectionFloat(this, ref x1, ref y1, ref x2, ref y2);
        if (valid)
        {
            result = (new FPoint(x1, y1), new FPoint(x2, y2));
            return true;
        }

        result = null;
        return false;
    }

    /// <summary>
    /// Attempts to find the union of this rectangle with another rectangle.
    /// </summary>
    /// <param name="other">The other rectangle to check for union.</param>
    /// <param name="result">The union rectangle, if found.</param>
    /// <returns>True if the union is found, otherwise false.</returns>
    public bool TryUnion(FRect other, out FRect? result)
    {
        if (SDL_GetRectUnionFloat(this, other, out var union))
        {
            result = union;
            return true;
        }

        result = null;
        return false;
    }
}
