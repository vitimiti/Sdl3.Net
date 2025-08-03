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

namespace Sdl3.Net.Video.Blending;

/// <summary>
/// Represents the blend operation used for custom blend modes.
/// </summary>
public enum BlendOperation
{
    /// <summary>
    /// Adds the source pixel to the destination pixel.
    /// </summary>
    /// <remarks>dst + src - Supported by all renderers.</remarks>
    Add = 1,

    /// <summary>
    /// Subtracts the source pixel from the destination pixel.
    /// </summary>
    /// <remarks>dst - src - Supported by D3D, OpenGL, OpenGLES, and Vulkan.</remarks>
    Subtract = 2,

    /// <summary>
    /// Subtracts the destination pixel from the source pixel.
    /// </summary>
    /// <remarks>src - dst - Supported by D3D, OpenGL, OpenGLES, and Vulkan.</remarks>
    ReverseSubtract = 3,

    /// <summary>
    /// Takes the minimum of the source and destination pixels.
    /// </summary>
    /// <remarks>min(dst, src) - Supported by D3D, OpenGL, OpenGLES, and Vulkan.</remarks>
    Minimum = 4,

    /// <summary>
    /// Takes the maximum of the source and destination pixels.
    /// </summary>
    /// <remarks>max(dst, src) - Supported by D3D, OpenGL, OpenGLES, and Vulkan.</remarks>
    Maximum = 5,
}
