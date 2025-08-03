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

namespace Sdl3.Net.Video.Blending;

/// <summary>
/// Allows the composition of custom blend modes.
/// </summary>
public static class CustomBlendMode
{
    /// <summary>
    /// Composes a custom blend mode using the specified color and alpha factors and operations.
    /// </summary>
    /// <param name="color">The color blend factors and operation.</param>
    /// <param name="alpha">The alpha blend factors and operation.</param>
    /// <returns>A <see cref="BlendMode"/> representing the composed custom blend mode.</returns>
    public static BlendMode Compose(
        (BlendFactor Source, BlendFactor Destination, BlendOperation Operation) color,
        (BlendFactor Source, BlendFactor Destination, BlendOperation Operation) alpha
    )
    {
        return SDL_ComposeCustomBlendMode(
            color.Source,
            color.Destination,
            color.Operation,
            alpha.Source,
            alpha.Destination,
            alpha.Operation
        );
    }
}
