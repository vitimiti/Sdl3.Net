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

namespace Sdl3.Net.PropertiesSystem;

/// <summary>
/// Represents the type of a property in the SDL properties system.
/// </summary>
public enum PropertyType
{
    /// <summary>
    ///  Indicates an invalid property type.
    /// </summary>
    Invalid,

    /// <summary>
    ///  Represents a property that is a pointer to some data.
    /// </summary>
    Pointer,

    /// <summary>
    ///  Represents a property that is a string.
    /// </summary>
    String,

    /// <summary>
    ///  Represents a property that is a number.
    /// </summary>
    Number,

    /// <summary>
    ///  Represents a property that is a floating-point number.
    /// </summary>
    Float,

    /// <summary>
    ///  Represents a property that is a boolean value.
    /// </summary>
    Boolean,
}
