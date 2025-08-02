using System.Runtime.InteropServices;
using Sdl3.Net.Extensions;
using Sdl3.Net.PropertiesSystem;
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.StreamManagement;

public class IoStream : Stream
{
    private readonly SDL_IOStream _ioStream;
    private bool _disposedValue;

    private IoStream(SDL_IOStream ioStream) => _ioStream = ioStream;

    protected override void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // Free managed resources
                _ioStream.Dispose();
            }

            // Free unmanaged resources
            _disposedValue = true;
        }

        base.Dispose(disposing);
    }

    public override bool CanRead =>
        Status is not IoStatus.Error and IoStatus.Ready and not IoStatus.WriteOnly;

    public override bool CanSeek => Status is not IoStatus.Error and IoStatus.Ready;

    public override bool CanWrite =>
        Status is not IoStatus.Error and IoStatus.Ready and not IoStatus.ReadOnly;

    public override long Length => SDL_GetIOSize(_ioStream);

    public override long Position
    {
        get => SDL_TellIO(_ioStream);
        set => Seek(value, SeekOrigin.Begin);
    }

    public IoStatus Status => SDL_GetIOStatus(_ioStream);

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
        if (ioStream.IsInvalid)
        {
            throw new ExternalException(
                $"Failed to open file '{file}' with mode '{modeString}': {SDL_GetError()}"
            );
        }

        return new IoStream(ioStream);
    }

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

    public static IoStream FromDynamicMemory()
    {
        SDL_IOStream ioStream = SDL_IOFromDynamicMem();
        return ioStream.IsInvalid
            ? throw new ExternalException($"Failed to create dynamic IO stream: {SDL_GetError()}")
            : new IoStream(ioStream);
    }

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

    public IoStreamProperties GetProperties()
    {
        SDL_PropertiesID propertiesId = SDL_GetIOProperties(_ioStream);
        return propertiesId.Value == 0
            ? throw new ExternalException($"Failed to get IO stream properties: {SDL_GetError()}")
            : new IoStreamProperties(propertiesId);
    }

    public override void Flush()
    {
        if (!SDL_FlushIO(_ioStream))
        {
            throw new ExternalException($"Failed to flush IO stream: {SDL_GetError()}");
        }
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        unsafe
        {
            fixed (byte* bufferPtr = buffer)
            {
                var bytesRead = SDL_ReadIO(_ioStream, bufferPtr + offset, new CULong((nuint)count));
                return bytesRead <= 0 && Status is not IoStatus.Eof
                    ? throw new ExternalException(
                        $"Failed to read from IO stream: {SDL_GetError()}"
                    )
                    : bytesRead;
            }
        }
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        var finalOffset = SDL_SeekIO(_ioStream, offset, (IoWhence)origin);
        return offset < 0
            ? throw new ExternalException($"Failed to seek in IO stream: {SDL_GetError()}")
            : finalOffset;
    }

    public override void SetLength(long value) =>
        throw new NotSupportedException("Setting length is not supported for IO streams.");

    public override void Write(byte[] buffer, int offset, int count)
    {
        unsafe
        {
            fixed (byte* bufferPtr = buffer)
            {
                var bytesWritten = SDL_WriteIO(
                    _ioStream,
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

    public void Print(string str)
    {
        if (SDL_IOprintf(_ioStream, str.ToStdIoString()).Value == 0)
        {
            throw new ExternalException($"Failed to print '{str}' to IO stream: {SDL_GetError()}");
        }
    }

    public Memory<byte> Load()
    {
        unsafe
        {
            var dataPtr = SDL_LoadFile_IO(_ioStream, out var dataSize, closeio: false);
            if (dataPtr is null)
            {
                throw new ExternalException(
                    $"Failed to load data from IO stream: {SDL_GetError()}"
                );
            }

            return new Span<byte>(dataPtr, (int)dataSize.Value).ToArray().AsMemory();
        }
    }

    public void Save(ReadOnlyMemory<byte> data)
    {
        unsafe
        {
            fixed (byte* dataPtr = data.Span)
            {
                if (
                    !SDL_SaveFile_IO(
                        _ioStream,
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

    public byte ReadU8() =>
        !SDL_ReadU8(_ioStream, out var value)
            ? throw new ExternalException($"Failed to read byte from IO stream: {SDL_GetError()}")
            : value;

    public sbyte ReadS8() =>
        !SDL_ReadS8(_ioStream, out var value)
            ? throw new ExternalException($"Failed to read sbyte from IO stream: {SDL_GetError()}")
            : value;

    public ushort ReadU16LittleEndian() =>
        !SDL_ReadU16LE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian ushort from IO stream: {SDL_GetError()}"
            )
            : value;

    public short ReadS16LittleEndian() =>
        !SDL_ReadS16LE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian short from IO stream: {SDL_GetError()}"
            )
            : value;

    public ushort ReadU16BigEndian() =>
        !SDL_ReadU16BE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian ushort from IO stream: {SDL_GetError()}"
            )
            : value;

    public short ReadS16BigEndian() =>
        !SDL_ReadS16BE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian short from IO stream: {SDL_GetError()}"
            )
            : value;

    public uint ReadU32LittleEndian() =>
        !SDL_ReadU32LE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian uint from IO stream: {SDL_GetError()}"
            )
            : value;

    public int ReadS32LittleEndian() =>
        !SDL_ReadS32LE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian int from IO stream: {SDL_GetError()}"
            )
            : value;

    public uint ReadU32BigEndian() =>
        !SDL_ReadU32BE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian uint from IO stream: {SDL_GetError()}"
            )
            : value;

    public int ReadS32BigEndian() =>
        !SDL_ReadS32BE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian int from IO stream: {SDL_GetError()}"
            )
            : value;

    public ulong ReadU64LittleEndian() =>
        !SDL_ReadU64LE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian ulong from IO stream: {SDL_GetError()}"
            )
            : value;

    public long ReadS64LittleEndian() =>
        !SDL_ReadS64LE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read little endian long from IO stream: {SDL_GetError()}"
            )
            : value;

    public ulong ReadU64BigEndian() =>
        !SDL_ReadU64BE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian ulong from IO stream: {SDL_GetError()}"
            )
            : value;

    public long ReadS64BigEndian() =>
        !SDL_ReadS64BE(_ioStream, out var value)
            ? throw new ExternalException(
                $"Failed to read big endian long from IO stream: {SDL_GetError()}"
            )
            : value;

    public void WriteU8(byte value)
    {
        if (!SDL_WriteU8(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write byte '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteS8(sbyte value)
    {
        if (!SDL_WriteS8(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write sbyte '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteU16LittleEndian(ushort value)
    {
        if (!SDL_WriteU16LE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write little endian ushort '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteS16LittleEndian(short value)
    {
        if (!SDL_WriteS16LE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write little endian short '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteU16BigEndian(ushort value)
    {
        if (!SDL_WriteU16BE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write big endian ushort '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteS16BigEndian(short value)
    {
        if (!SDL_WriteS16BE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write big endian short '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteU32LittleEndian(uint value)
    {
        if (!SDL_WriteU32LE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write little endian uint '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteU32BigEndian(uint value)
    {
        if (!SDL_WriteU32BE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write big endian uint '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteS32LittleEndian(int value)
    {
        if (!SDL_WriteS32LE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write little endian int '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteS32BigEndian(int value)
    {
        if (!SDL_WriteS32BE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write big endian int '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteU64LittleEndian(ulong value)
    {
        if (!SDL_WriteU64LE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write little endian ulong '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteU64BigEndian(ulong value)
    {
        if (!SDL_WriteU64BE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write big endian ulong '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteS64LittleEndian(long value)
    {
        if (!SDL_WriteS64LE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write little endian long '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }

    public void WriteS64BigEndian(long value)
    {
        if (!SDL_WriteS64BE(_ioStream, value))
        {
            throw new ExternalException(
                $"Failed to write big endian long '{value}' to IO stream: {SDL_GetError()}"
            );
        }
    }
}
