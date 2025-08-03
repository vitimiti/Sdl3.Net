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
using Sdl3.Net.Extensions;
using Sdl3.Net.PropertiesSystem;
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.StreamManagement;

/// <summary>
/// Represents an I/O stream in SDL.
/// </summary>
public class IoStream : Stream
{
    private readonly SDL_IOStream _handle;
    private bool _disposedValue;

    private IoStream(SDL_IOStream ioStream) => _handle = ioStream;

    /// <summary>
    ///  Disposes the resources of the <see cref="IoStream"/> class.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // Free managed resources
                _handle.Dispose();
            }

            // Free unmanaged resources
            _disposedValue = true;
        }

        base.Dispose(disposing);
    }

    /// <summary>
    ///  Gets a value indicating whether the I/O stream can be read from.
    /// </summary>
    public override bool CanRead =>
        Status is not IoStatus.Error and IoStatus.Ready and not IoStatus.WriteOnly;

    /// <summary>
    ///  Gets a value indicating whether the I/O stream can be seeked.
    /// </summary>
    public override bool CanSeek => Status is not IoStatus.Error and IoStatus.Ready;

    /// <summary>
    ///  Gets a value indicating whether the I/O stream can be written to.
    /// </summary>
    public override bool CanWrite =>
        Status is not IoStatus.Error and IoStatus.Ready and not IoStatus.ReadOnly;

    /// <summary>
    ///  Gets the length of the I/O stream.
    /// </summary>
    public override long Length => SDL_GetIOSize(_handle);

    /// <summary>
    /// Gets or sets the current position in the I/O stream.
    /// </summary>
    public override long Position
    {
        get => SDL_TellIO(_handle);
        set => Seek(value, SeekOrigin.Begin);
    }

    /// <summary>
    /// Gets the status of the I/O stream.
    /// </summary>
    /// <returns>The status of the I/O stream.</returns>
    public IoStatus Status => SDL_GetIOStatus(_handle);

    /// <summary>
    /// Creates an I/O stream from a file.
    /// </summary>
    /// <param name="file">The file path.</param>
    /// <param name="mode">The file mode.</param>
    /// <returns>The created I/O stream.</returns>
    /// <exception cref="ExternalException">Thrown if the file could not be opened.</exception>
    public static IoStream FromFile(string file, IoStreamFileMode mode)
    {
        var modeString = mode switch
        {
            IoStreamFileMode.OpenAndRead => "r",
            IoStreamFileMode.CreateOrTruncateAndWrite => "w",
            IoStreamFileMode.AppendOrCreateAndAppend => "a",
            IoStreamFileMode.OpenAndReadWrite => "r+",
            IoStreamFileMode.CreateOrTruncateAndReadWrite => "w+",
            IoStreamFileMode.AppendOrCreateAndReadAppend => "a+",
            IoStreamFileMode.BinaryOpenAndRead => "rb",
            IoStreamFileMode.BinaryCreateOrTruncateAndWrite => "wb",
            IoStreamFileMode.BinaryAppendOrCreateAndAppend => "ab",
            IoStreamFileMode.BinaryOpenAndReadWrite => "r+b",
            IoStreamFileMode.BinaryCreateOrTruncateAndReadWrite => "w+b",
            IoStreamFileMode.BinaryAppendOrCreateAndReadAppend => "a+b",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };

        SDL_IOStream ioStream = SDL_IOFromFile(file, modeString);
        return ioStream.IsInvalid
            ? throw new ExternalException(
                $"Failed to open file '{file}' with mode '{modeString}': {SDL_GetError()}"
            )
            : new IoStream(ioStream);
    }

    /// <summary>
    /// Creates an I/O stream from a memory buffer.
    /// </summary>
    /// <param name="memory">The memory buffer.</param>
    /// <returns>The created I/O stream.</returns>
    /// <exception cref="ExternalException">Thrown if the memory could not be used to create an I/O stream.</exception>
    public static IoStream FromMemory(Memory<byte> memory)
    {
        unsafe
        {
            fixed (byte* memPtr = memory.Span)
            {
                SDL_IOStream ioStream = SDL_IOFromMem(memPtr, new CULong((nuint)memory.Length));
                return ioStream.IsInvalid
                    ? throw new ExternalException(
                        $"Failed to create IO stream from memory: {SDL_GetError()}"
                    )
                    : new IoStream(ioStream);
            }
        }
    }

    /// <summary>
    /// Creates an I/O stream from a constant memory buffer.
    /// </summary>
    /// <param name="memory">The memory buffer.</param>
    /// <returns>The created I/O stream.</returns>
    /// <exception cref="ExternalException">Thrown if the memory could not be used to create an I/O stream.</exception>
    public static IoStream FromConstMemory(ReadOnlyMemory<byte> memory)
    {
        unsafe
        {
            fixed (byte* memPtr = memory.Span)
            {
                SDL_IOStream ioStream = SDL_IOFromConstMem(
                    memPtr,
                    new CULong((nuint)memory.Length)
                );
                return ioStream.IsInvalid
                    ? throw new ExternalException(
                        $"Failed to create IO stream from const memory: {SDL_GetError()}"
                    )
                    : new IoStream(ioStream);
            }
        }
    }

    /// <summary>
    /// Creates an I/O stream from dynamic memory.
    /// </summary>
    /// <returns>The created I/O stream.</returns>
    /// <exception cref="ExternalException">Thrown if the dynamic memory could not be used to create an I/O stream.</exception>
    public static IoStream FromDynamicMemory()
    {
        SDL_IOStream ioStream = SDL_IOFromDynamicMem();
        return ioStream.IsInvalid
            ? throw new ExternalException($"Failed to create dynamic IO stream: {SDL_GetError()}")
            : new IoStream(ioStream);
    }

    /// <summary>
    /// Loads a file into memory.
    /// </summary>
    /// <param name="file">The file path.</param>
    /// <returns>The loaded file data.</returns>
    /// <exception cref="ExternalException">Thrown if the file could not be loaded.</exception>
    public static Memory<byte> LoadFile(string file)
    {
        unsafe
        {
            var dataPtr = SDL_LoadFile(file, out var size);
            if (dataPtr is null)
            {
                throw new ExternalException($"Failed to load file '{file}': {SDL_GetError()}");
            }

            return new Span<byte>(dataPtr, (int)size.Value).ToArray().AsMemory();
        }
    }

    /// <summary>
    /// Saves data to a file.
    /// </summary>
    /// <param name="file">The file path.</param>
    /// <param name="data">The data to save.</param>
    /// <exception cref="ExternalException">Thrown if the file could not be saved.</exception>
    public static void SaveFile(string file, ReadOnlyMemory<byte> data)
    {
        unsafe
        {
            fixed (byte* dataPtr = data.Span)
            {
                if (!SDL_SaveFile(file, dataPtr, new CULong((nuint)data.Length)))
                {
                    throw new ExternalException($"Failed to save file '{file}': {SDL_GetError()}");
                }
            }
        }
    }

    /// <summary>
    /// Gets the properties of the I/O stream.
    /// </summary>
    /// <returns>The properties of the I/O stream.</returns>
    /// <exception cref="ExternalException">Thrown if the properties could not be retrieved.</exception>
    public IoStreamProperties GetProperties()
    {
        SDL_PropertiesID propertiesId = SDL_GetIOProperties(_handle);
        return propertiesId.Value == 0
            ? throw new ExternalException($"Failed to get IO stream properties: {SDL_GetError()}")
            : new IoStreamProperties(propertiesId);
    }

    /// <summary>
    ///  Flushes the I/O stream.
    /// </summary>
    /// <exception cref="ExternalException">Thrown if the I/O stream could not be flushed.</exception>
    public override void Flush()
    {
        if (!SDL_FlushIO(_handle))
        {
            throw new ExternalException($"Failed to flush IO stream: {SDL_GetError()}");
        }
    }

    /// <summary>
    /// Reads data from the I/O stream into a byte array.
    /// </summary>
    /// <param name="buffer">The buffer to read data into.</param>
    /// <param name="offset">The offset in the buffer to start writing data.</param>
    /// <param name="count">The number of bytes to read.</param>
    /// <returns>The number of bytes read.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public override int Read(byte[] buffer, int offset, int count)
    {
        unsafe
        {
            fixed (byte* bufferPtr = buffer)
            {
                var bytesRead = SDL_ReadIO(_handle, bufferPtr + offset, new CULong((nuint)count));
                return bytesRead <= 0 && Status is not IoStatus.Eof
                    ? throw new ExternalException(
                        $"Failed to read from IO stream: {SDL_GetError()}"
                    )
                    : bytesRead;
            }
        }
    }

    /// <summary>
    /// Seeks to a specific position in the I/O stream.
    /// </summary>
    /// <param name="offset">The offset to seek to.</param>
    /// <param name="origin">The seek origin.</param>
    /// <returns>The new position in the I/O stream.</returns>
    /// <exception cref="ExternalException">Thrown if the seek operation fails.</exception>
    public override long Seek(long offset, SeekOrigin origin)
    {
        var finalOffset = SDL_SeekIO(_handle, offset, (IoWhence)origin);
        return offset < 0
            ? throw new ExternalException($"Failed to seek in IO stream: {SDL_GetError()}")
            : finalOffset;
    }

    /// <summary>
    /// This operation is NOT supported.
    /// </summary>
    /// <param name="value">The new length of the I/O stream.</param>
    /// <exception cref="NotSupportedException">Always thrown.</exception>
    public override void SetLength(long value) =>
        throw new NotSupportedException("Setting length is not supported for IO streams.");

    /// <summary>
    /// Writes data to the I/O stream from a byte array.
    /// </summary>
    /// <param name="buffer">The buffer containing the data to write.</param>
    /// <param name="offset">The offset in the buffer to start reading data.</param>
    /// <param name="count">The number of bytes to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
        unsafe
        {
            fixed (byte* bufferPtr = buffer)
            {
                var bytesWritten = SDL_WriteIO(
                    _handle,
                    bufferPtr + offset,
                    new CULong((nuint)count)
                );

                if (bytesWritten != count)
                {
                    throw new ExternalException($"Failed to write to IO stream: {SDL_GetError()}");
                }
            }
        }
    }

    /// <summary>
    /// Prints a string to the I/O stream.
    /// </summary>
    /// <param name="str">The string to print.</param>
    /// <exception cref="ExternalException">Thrown if the print operation fails.</exception>
    public void Print(string str)
    {
        if (SDL_IOprintf(_handle, str.ToStdIoString()).Value == 0)
        {
            throw new ExternalException($"Failed to print '{str}' to IO stream: {SDL_GetError()}");
        }
    }

    /// <summary>
    /// Loads data from the I/O stream into a memory buffer.
    /// </summary>
    /// <returns>A memory buffer containing the loaded data.</returns>
    /// <exception cref="ExternalException">Thrown if the load operation fails.</exception>
    public Memory<byte> Load()
    {
        unsafe
        {
            var dataPtr = SDL_LoadFile_IO(_handle, out var dataSize, closeio: false);
            if (dataPtr is null)
            {
                throw new ExternalException(
                    $"Failed to load data from IO stream: {SDL_GetError()}"
                );
            }

            return new Span<byte>(dataPtr, (int)dataSize.Value).ToArray().AsMemory();
        }
    }

    /// <summary>
    /// Saves data to the I/O stream from a memory buffer.
    /// </summary>
    /// <param name="data">The memory buffer containing the data to save.</param>
    /// <exception cref="ExternalException">Thrown if the save operation fails.</exception>
    public void Save(ReadOnlyMemory<byte> data)
    {
        unsafe
        {
            fixed (byte* dataPtr = data.Span)
            {
                if (
                    !SDL_SaveFile_IO(
                        _handle,
                        dataPtr,
                        new CULong((nuint)data.Length),
                        closeio: false
                    )
                )
                {
                    throw new ExternalException(
                        $"Failed to save data to IO stream: {SDL_GetError()}"
                    );
                }
            }
        }
    }

    /// <summary>
    /// Reads an unsigned 8-bit integer from the I/O stream.
    /// </summary>
    /// <returns>The read unsigned 8-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public byte ReadU8() =>
        !SDL_ReadU8(_handle, out var value)
            ? throw new ExternalException($"Failed to read byte from IO stream: {SDL_GetError()}")
            : value;

    /// <summary>
    /// Reads a signed 8-bit integer from the I/O stream.
    /// </summary>
    /// <returns>The read signed 8-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public sbyte ReadS8() =>
        !SDL_ReadS8(_handle, out var value)
            ? throw new ExternalException($"Failed to read sbyte from IO stream: {SDL_GetError()}")
            : value;

    /// <summary>
    /// Reads an unsigned 16-bit integer in little-endian format from the I/O stream
    /// </summary>
    /// <returns>The read unsigned 16-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public ushort ReadU16LittleEndian() =>
        !SDL_ReadU16LE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian ushort from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Reads a signed 16-bit integer in little-endian format from the I/O stream.
    /// </summary>
    /// <returns>The read signed 16-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public short ReadS16LittleEndian() =>
        !SDL_ReadS16LE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian short from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Reads an unsigned 16-bit integer in big-endian format from the I/O stream.
    /// </summary>
    /// <returns>The read unsigned 16-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public ushort ReadU16BigEndian() =>
        !SDL_ReadU16BE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian ushort from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Reads a signed 16-bit integer in big-endian format from the I/O stream.
    /// </summary>
    /// <returns>The read signed 16-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public short ReadS16BigEndian() =>
        !SDL_ReadS16BE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian short from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Reads an unsigned 32-bit integer in little-endian format from the I/O stream.
    /// </summary>
    /// <returns>The read unsigned 32-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public uint ReadU32LittleEndian() =>
        !SDL_ReadU32LE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian uint from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Reads a signed 32-bit integer in little-endian format from the I/O stream.
    /// </summary>
    /// <returns>The read signed 32-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public int ReadS32LittleEndian() =>
        !SDL_ReadS32LE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian int from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Reads an unsigned 32-bit integer in big-endian format from the I/O stream.
    /// </summary>
    /// <returns>The read unsigned 32-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public uint ReadU32BigEndian() =>
        !SDL_ReadU32BE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian uint from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Reads a signed 32-bit integer in big-endian format from the I/O stream.
    /// </summary>
    /// <returns>The read signed 32-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public int ReadS32BigEndian() =>
        !SDL_ReadS32BE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian int from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Reads an unsigned 64-bit integer in little-endian format from the I/O stream.
    /// </summary>
    /// <returns>The read unsigned 64-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public ulong ReadU64LittleEndian() =>
        !SDL_ReadU64LE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian ulong from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Reads a signed 64-bit integer in little-endian format from the I/O stream.
    /// </summary>
    /// <returns>The read signed 64-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public long ReadS64LittleEndian() =>
        !SDL_ReadS64LE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian long from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Reads an unsigned 64-bit integer in big-endian format from the I/O stream.
    /// </summary>
    /// <returns>The read unsigned 64-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public ulong ReadU64BigEndian() =>
        !SDL_ReadU64BE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian ulong from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Reads a signed 64-bit integer in big-endian format from the I/O stream.
    /// </summary>
    /// <returns>The read signed 64-bit integer.</returns>
    /// <exception cref="ExternalException">Thrown if the read operation fails.</exception>
    public long ReadS64BigEndian() =>
        !SDL_ReadS64BE(_handle, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian long from IO stream: {SDL_GetError()}"
            )
            : value;

    /// <summary>
    /// Writes an unsigned 8-bit integer to the I/O stream.
    /// </summary>
    /// <param name="value">The unsigned 8-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteU8(byte value)
    {
        if (!SDL_WriteU8(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write byte '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes a signed 8-bit integer to the I/O stream.
    /// </summary>
    /// <param name="value">The signed 8-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteS8(sbyte value)
    {
        if (!SDL_WriteS8(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write sbyte '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes an unsigned 16-bit integer in little-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The unsigned 16-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteU16LittleEndian(ushort value)
    {
        if (!SDL_WriteU16LE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write little endian ushort '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes a signed 16-bit integer in little-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The signed 16-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteS16LittleEndian(short value)
    {
        if (!SDL_WriteS16LE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write little endian short '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes an unsigned 16-bit integer in big-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The unsigned 16-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteU16BigEndian(ushort value)
    {
        if (!SDL_WriteU16BE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write big endian ushort '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes a signed 16-bit integer in big-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The signed 16-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteS16BigEndian(short value)
    {
        if (!SDL_WriteS16BE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write big endian short '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes an unsigned 32-bit integer in little-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The unsigned 32-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteU32LittleEndian(uint value)
    {
        if (!SDL_WriteU32LE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write little endian uint '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes a signed 32-bit integer in little-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The signed 32-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteU32BigEndian(uint value)
    {
        if (!SDL_WriteU32BE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write big endian uint '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes a signed 32-bit integer in big-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The signed 32-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteS32LittleEndian(int value)
    {
        if (!SDL_WriteS32LE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write little endian int '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes a signed 32-bit integer in big-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The signed 32-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteS32BigEndian(int value)
    {
        if (!SDL_WriteS32BE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write big endian int '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes an unsigned 64-bit integer in little-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The unsigned 64-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteU64LittleEndian(ulong value)
    {
        if (!SDL_WriteU64LE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write little endian ulong '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes a signed 64-bit integer in little-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The signed 64-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteU64BigEndian(ulong value)
    {
        if (!SDL_WriteU64BE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write big endian ulong '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes a signed 64-bit integer in big-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The signed 64-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteS64LittleEndian(long value)
    {
        if (!SDL_WriteS64LE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write little endian long '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    /// Writes a signed 64-bit integer in big-endian format to the I/O stream.
    /// </summary>
    /// <param name="value">The signed 64-bit integer to write.</param>
    /// <exception cref="ExternalException">Thrown if the write operation fails.</exception>
    public void WriteS64BigEndian(long value)
    {
        if (!SDL_WriteS64BE(_handle, value))
        {
            throw new ExternalException(
                $"Failed to write big endian long '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }
}
