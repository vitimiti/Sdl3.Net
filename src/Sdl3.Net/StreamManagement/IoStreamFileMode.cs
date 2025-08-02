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

namespace Sdl3.Net.StreamManagement;

/// <summary>
/// Represents the file mode for I/O streams in SDL.
/// </summary>
public enum IoStreamFileMode
{
    /// <summary>
    ///  Opens an existing file for reading.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "r" mode.
    /// </remarks>
    OpenAndRead,

    /// <summary>
    ///  Creates a new file or truncates an existing file for writing.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "w" mode.
    /// </remarks>
    CreateOrTruncateAndWrite,

    /// <summary>
    ///  Opens an existing file for appending.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "a" mode.
    /// </remarks>
    AppendOrCreateAndAppend,

    /// <summary>
    ///  Opens an existing file for reading and writing.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "r+" mode.
    /// </remarks>
    OpenAndReadWrite,

    /// <summary>
    ///  Creates a new file or truncates an existing file for reading and writing.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "w+" mode.
    /// </remarks>
    CreateOrTruncateAndReadWrite,

    /// <summary>
    ///  Creates a new file or appends to an existing file for reading and writing.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "a+" mode.
    /// </remarks>
    AppendOrCreateAndReadAppend,

    /// <summary>
    ///  Opens an existing file for reading in binary mode.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "rb" mode.
    /// </remarks>
    BinaryOpenAndRead,

    /// <summary>
    ///  Creates a new file or truncates an existing file for writing in binary mode.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "wb" mode.
    /// </remarks>
    BinaryCreateOrTruncateAndWrite,

    /// <summary>
    ///  Opens an existing file for appending in binary mode.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "ab" mode.
    /// </remarks>
    BinaryAppendOrCreateAndAppend,

    /// <summary>
    ///  Opens an existing file for reading and writing in binary mode.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "r+b" mode.
    /// </remarks>
    BinaryOpenAndReadWrite,

    /// <summary>
    ///  Creates a new file or truncates an existing file for reading and writing in binary mode.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "w+b" mode.
    /// </remarks>
    BinaryCreateOrTruncateAndReadWrite,

    /// <summary>
    ///  Creates a new file or appends to an existing file for reading and writing in binary mode.
    /// </summary>
    /// <remarks>
    /// This is equivalent to the POSIX "a+b" mode.
    /// </remarks>
    BinaryAppendOrCreateAndReadAppend,
}
