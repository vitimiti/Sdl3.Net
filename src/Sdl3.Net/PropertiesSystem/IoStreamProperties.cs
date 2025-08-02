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

namespace Sdl3.Net.PropertiesSystem;

/// <summary>
/// Represents properties related to I/O streams in the SDL properties system.
/// </summary>
public sealed class IoStreamProperties : Properties
{
    /// <summary>
    /// Gets the pointer to the Windows handle for the I/O stream.
    /// </summary>
    public nint WindowsHandlePointer =>
        GetPointerProperty(SDL_PROP_IOSTREAM_WINDOWS_HANDLE_POINTER);

    /// <summary>
    /// Gets the pointer to the POSIX file descriptor for the I/O stream.
    /// </summary>
    public nint StdioFilePointer => GetPointerProperty(SDL_PROP_IOSTREAM_STDIO_FILE_POINTER);

    /// <summary>
    /// Gets the pointer to the POSIX file descriptor for the I/O stream.
    /// </summary>
    public long FileDescriptor => GetNumberProperty(SDL_PROP_IOSTREAM_FILE_DESCRIPTOR_NUMBER);

    /// <summary>
    /// Gets the pointer to the Android asset for the I/O stream.
    /// </summary>
    public nint AndroidAAssetPointer =>
        GetPointerProperty(SDL_PROP_IOSTREAM_ANDROID_AASSET_POINTER);

    /// <summary>
    /// Gets the pointer to the memory for the I/O stream.
    /// </summary>
    /// <remarks>
    /// This is used for streams created with <see cref="StreamManagement.IoStream.FromMemory(Memory{byte})"/>
    /// and <see cref="StreamManagement.IoStream.FromConstMemory(ReadOnlyMemory{byte})"/>.
    /// </remarks>
    public nint MemoryPointer => GetPointerProperty(SDL_PROP_IOSTREAM_MEMORY_POINTER);

    /// <summary>
    /// Gets the size of the memory for the I/O stream.
    /// </summary>
    /// <remarks>
    /// This is used for streams created with <see cref="StreamManagement.IoStream.FromMemory(Memory{byte})"/>
    /// and <see cref="StreamManagement.IoStream.FromConstMemory(ReadOnlyMemory{byte})"/>.
    /// </remarks>
    public long MemorySize => GetNumberProperty(SDL_PROP_IOSTREAM_MEMORY_SIZE_NUMBER);

    /// <summary>
    /// Gets the pointer to the dynamic memory for the I/O stream.
    /// </summary>
    /// <remarks>
    /// This is used for streams created with <see cref="StreamManagement.IoStream.FromDynamicMemory"/>.
    /// </remarks>
    public nint DynamicMemoryPointer =>
        GetPointerProperty(SDL_PROP_IOSTREAM_DYNAMIC_MEMORY_POINTER);

    /// <summary>
    /// Gets the size of the dynamic memory for the I/O stream.
    /// </summary>
    /// <remarks>
    /// This is used for streams created with <see cref="StreamManagement.IoStream.FromDynamicMemory"/>.
    /// </remarks>
    public long DynamicChunkSize => GetNumberProperty(SDL_PROP_IOSTREAM_DYNAMIC_CHUNKSIZE_NUMBER);

    internal IoStreamProperties(SDL_PropertiesID propertiesId)
        : base(propertiesId) { }
}
