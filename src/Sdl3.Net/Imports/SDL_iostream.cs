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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Sdl3.Net.StreamManagement;

namespace Sdl3.Net.Imports;

internal static partial class SDL3
{
    [NativeMarshalling(typeof(SafeHandleMarshaller<SDL_IOStream>))]
    public sealed class SDL_IOStream() : SafeHandle(invalidHandleValue: nint.Zero, ownsHandle: true)
    {
        public override bool IsInvalid => handle == nint.Zero;

        protected override bool ReleaseHandle()
        {
            SDL_CloseIO(this);
            return true;
        }
    }

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_IOFromFile),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_IOStream SDL_IOFromFile(string file, string mode);

    public const string SDL_PROP_IOSTREAM_WINDOWS_HANDLE_POINTER = "SDL.iostream.windows.handle";
    public const string SDL_PROP_IOSTREAM_STDIO_FILE_POINTER = "SDL.iostream.stdio.file";
    public const string SDL_PROP_IOSTREAM_FILE_DESCRIPTOR_NUMBER = "SDL.iostream.file_descriptor";
    public const string SDL_PROP_IOSTREAM_ANDROID_AASSET_POINTER = "SDL.iostream.android.aasset";

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_IOFromMem))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_IOStream SDL_IOFromMem(void* mem, CULong size);

    public const string SDL_PROP_IOSTREAM_MEMORY_POINTER = "SDL.iostream.memory.base";
    public const string SDL_PROP_IOSTREAM_MEMORY_SIZE_NUMBER = "SDL.iostream.memory.size";

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_IOFromConstMem))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_IOStream SDL_IOFromConstMem(void* mem, CULong size);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_IOFromDynamicMem))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_IOStream SDL_IOFromDynamicMem();

    public const string SDL_PROP_IOSTREAM_DYNAMIC_MEMORY_POINTER = "SDL.iostream.dynamic.memory";
    public const string SDL_PROP_IOSTREAM_DYNAMIC_CHUNKSIZE_NUMBER =
        "SDL.iostream.dynamic.chunksize";

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CloseIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_CloseIO(SDL_IOStream context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetIOProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_PropertiesID SDL_GetIOProperties(SDL_IOStream context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetIOStatus))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial IoStatus SDL_GetIOStatus(SDL_IOStream context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetIOSize))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long SDL_GetIOSize(SDL_IOStream context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SeekIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial int SDL_SeekIO(SDL_IOStream context, long offset, IoWhence whence);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_TellIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial long SDL_TellIO(SDL_IOStream context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial int SDL_ReadIO(SDL_IOStream context, void* ptr, CULong size);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial int SDL_WriteIO(SDL_IOStream context, void* ptr, CULong size);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_IOprintf),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial CULong SDL_IOprintf(SDL_IOStream context, string fmt);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_FlushIO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_FlushIO(SDL_IOStream context);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_LoadFile_IO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void* SDL_LoadFile_IO(
        SDL_IOStream src,
        out CULong datasize,
        [MarshalAs(UnmanagedType.U1)] bool closeio
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LoadFile),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void* SDL_LoadFile(string file, out CULong datasize);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SaveFile_IO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SaveFile_IO(
        SDL_IOStream dst,
        void* data,
        CULong size,
        [MarshalAs(UnmanagedType.U1)] bool closeio
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SaveFile),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SaveFile(string file, void* data, CULong size);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU8))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadU8(SDL_IOStream context, out byte value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS8))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadS8(SDL_IOStream context, out sbyte value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU16LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadU16LE(SDL_IOStream context, out ushort value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS16LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadS16LE(SDL_IOStream context, out short value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU16BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadU16BE(SDL_IOStream context, out ushort value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS16BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadS16BE(SDL_IOStream context, out short value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU32LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadU32LE(SDL_IOStream context, out uint value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS32LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadS32LE(SDL_IOStream context, out int value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU32BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadU32BE(SDL_IOStream context, out uint value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS32BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadS32BE(SDL_IOStream context, out int value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU64LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadU64LE(SDL_IOStream context, out ulong value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS64LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadS64LE(SDL_IOStream context, out long value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadU64BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadU64BE(SDL_IOStream context, out ulong value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadS64BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_ReadS64BE(SDL_IOStream context, out long value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU8))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteU8(SDL_IOStream context, byte value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS8))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteS8(SDL_IOStream context, sbyte value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU16LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteU16LE(SDL_IOStream context, ushort value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS16LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteS16LE(SDL_IOStream context, short value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU16BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteU16BE(SDL_IOStream context, ushort value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS16BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteS16BE(SDL_IOStream context, short value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU32LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteU32LE(SDL_IOStream context, uint value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS32LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteS32LE(SDL_IOStream context, int value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU32BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteU32BE(SDL_IOStream context, uint value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS32BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteS32BE(SDL_IOStream context, int value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU64LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteU64LE(SDL_IOStream context, ulong value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS64LE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteS64LE(SDL_IOStream context, long value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteU64BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteU64BE(SDL_IOStream context, ulong value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteS64BE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_WriteS64BE(SDL_IOStream context, long value);
}
