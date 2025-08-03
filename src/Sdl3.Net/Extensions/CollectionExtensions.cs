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
using Sdl3.Net.PropertiesSystem;
using Sdl3.Net.Shapes;
using Sdl3.Net.Video.Pixels;
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.Extensions;

/// <summary>
/// Provides extension methods for collections of points.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Gets a rectangle that encloses all points in the collection, optionally clipped to a specified rectangle.
    /// </summary>
    /// <param name="points">The collection of points to enclose.</param>
    /// <param name="clip">An optional rectangle to clip the enclosing rectangle.</param>
    /// <param name="result">The resulting rectangle that encloses the points, or null if no valid rectangle can be formed.</param>
    /// <returns>True if a valid rectangle was found, otherwise false.</returns>
    public static bool TryGetEnclosingRect(
        this IEnumerable<Point> points,
        Rect? clip,
        out Rect? result
    )
    {
        if (points is null || !points.Any())
        {
            result = null;
            return false;
        }

        var valid = SDL_GetRectEnclosingPoints([.. points], points.Count(), clip, out var rect);
        result = valid ? rect : null;
        return valid;
    }

    /// <summary>
    /// Gets a rectangle that encloses all points in the collection, optionally clipped to a specified rectangle.
    /// </summary>
    /// <param name="points">The collection of points to enclose.</param>
    /// <param name="clip">An optional rectangle to clip the enclosing rectangle.</param>
    /// <param name="result">The resulting rectangle that encloses the points, or null if no valid rectangle can be formed.</param>
    /// <returns>True if a valid rectangle was found, otherwise false.</returns>
    public static bool TryGetEnclosingFRect(
        this IEnumerable<FPoint> points,
        FRect? clip,
        out FRect? result
    )
    {
        if (points is null || !points.Any())
        {
            result = null;
            return false;
        }

        var valid = SDL_GetRectEnclosingPointsFloat(
            [.. points],
            points.Count(),
            clip,
            out var rect
        );

        result = valid ? rect : null;
        return valid;
    }

    /// <summary>
    /// Converts a collection of pixels to a different pixel format.
    /// </summary>
    /// <param name="source">The source pixel data.</param>
    /// <param name="size">The dimensions of the source pixel data.</param>
    /// <param name="sourceData">The pixel format and pitch of the source pixel data.</param>
    /// <param name="destinationData">The pixel format and pitch of the destination pixel data.</param>
    /// <returns>A memory block containing the converted pixel data.</returns>
    public static Memory<byte> ConvertPixels(
        this ReadOnlyMemory<byte> source,
        (int Width, int Height) size,
        (PixelFormat Format, int Pitch) sourceData,
        (PixelFormat Format, int Pitch) destinationData
    )
    {
        unsafe
        {
            var sourcePtr = (byte*)source.Span.GetPinnableReference();
            var destinationPtr = stackalloc byte[source.Length];

            if (
                !SDL_ConvertPixels(
                    size.Width,
                    size.Height,
                    sourceData.Format.Handle,
                    sourcePtr,
                    sourceData.Pitch,
                    destinationData.Format.Handle,
                    destinationPtr,
                    destinationData.Pitch
                )
            )
            {
                throw new ExternalException(
                    $"Failed to convert pixels with size '{size}', source data '{sourceData}' and destination data '{destinationData}': {SDL_GetError()}"
                );
            }

            return new Memory<byte>(new Span<byte>(destinationPtr, source.Length).ToArray());
        }
    }

    /// <summary>
    /// Converts a collection of pixels to a different pixel format and color space.
    /// </summary>
    /// <param name="source">The source pixel data.</param>
    /// <param name="size">The dimensions of the source pixel data.</param>
    /// <param name="sourceData">The pixel format, color space, properties, and pitch of the source pixel data.</param>
    /// <param name="destinationData">The pixel format, color space, properties, and pitch of the destination pixel data.</param>
    /// <returns>A memory block containing the converted pixel data.</returns>
    public static Memory<byte> ConvertPixels(
        this ReadOnlyMemory<byte> source,
        (int Width, int Height) size,
        (PixelFormat Format, ColorSpace colorSpace, Properties Props, int Pitch) sourceData,
        (PixelFormat Format, ColorSpace colorSpace, Properties Props, int Pitch) destinationData
    )
    {
        unsafe
        {
            var sourcePtr = (byte*)source.Span.GetPinnableReference();
            var destinationPtr = stackalloc byte[source.Length];

            if (
                !SDL_ConvertPixelsAndColorspace(
                    size.Width,
                    size.Height,
                    sourceData.Format.Handle,
                    sourceData.colorSpace.Handle,
                    sourceData.Props.Id,
                    sourcePtr,
                    sourceData.Pitch,
                    destinationData.Format.Handle,
                    destinationData.colorSpace.Handle,
                    destinationData.Props.Id,
                    destinationPtr,
                    destinationData.Pitch
                )
            )
            {
                throw new ExternalException(
                    $"Failed to convert pixels with size '{size}', source data '{sourceData}' and destination data '{destinationData}': {SDL_GetError()}"
                );
            }

            return new Memory<byte>(new Span<byte>(destinationPtr, source.Length).ToArray());
        }
    }

    /// <summary>
    /// Premultiplies the alpha channel of the pixel data.
    /// </summary>
    /// <param name="source">The source pixel data.</param>
    /// <param name="size">The dimensions of the source pixel data.</param>
    /// <param name="sourceData">The pixel format and pitch of the source pixel data.</param>
    /// <param name="destinationData">The pixel format and pitch of the destination pixel data.</param>
    /// <param name="linear">Indicates whether the operation should be performed in linear color space.</param>
    /// <returns>A memory block containing the pixel data with premultiplied alpha.</returns>
    /// <exception cref="ExternalException">Thrown if the operation fails.</exception>
    public static Memory<byte> PremultiplyPixelsAlpha(
        this ReadOnlyMemory<byte> source,
        (int Width, int Height) size,
        (PixelFormat Format, int Pitch) sourceData,
        (PixelFormat Format, int Pitch) destinationData,
        bool linear = false
    )
    {
        unsafe
        {
            var sourcePtr = (byte*)source.Span.GetPinnableReference();
            var destinationPtr = stackalloc byte[source.Length];

            if (
                !SDL_PremultiplyAlpha(
                    size.Width,
                    size.Height,
                    sourceData.Format.Handle,
                    sourcePtr,
                    sourceData.Pitch,
                    destinationData.Format.Handle,
                    destinationPtr,
                    destinationData.Pitch,
                    linear
                )
            )
            {
                throw new ExternalException(
                    $"Failed to premultiply pixels alpha with size '{size}', source data '{sourceData}' and destination data '{destinationData}': {SDL_GetError()}"
                );
            }

            return new Memory<byte>(new Span<byte>(destinationPtr, source.Length).ToArray());
        }
    }
}
