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

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Sdl3.Net.CustomMarshallers;
using Sdl3.Net.Video.Pixels;

namespace Sdl3.Net.Imports;

internal static partial class SDL3
{
    public enum SDL_PixelFormat : uint;

    public static SDL_PixelFormat SDL_PIXELFORMAT_UNKNOWN => 0U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_INDEX1LSB => (SDL_PixelFormat)0x11100100U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_INDEX1MSB => (SDL_PixelFormat)0x11200100U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_INDEX2LSB => (SDL_PixelFormat)0x1C100200U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_INDEX2MSB => (SDL_PixelFormat)0x1C200200U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_INDEX4LSB => (SDL_PixelFormat)0x12100400U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_INDEX4MSB => (SDL_PixelFormat)0x12200400U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_INDEX8 => (SDL_PixelFormat)0x13000801U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGB332 => (SDL_PixelFormat)0x14110801U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_XRGB4444 => (SDL_PixelFormat)0x15120C02U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_XBGR4444 => (SDL_PixelFormat)0x15520C02U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_XRGB1555 => (SDL_PixelFormat)0x15130F02U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_XBGR1555 => (SDL_PixelFormat)0x15530F02U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ARGB4444 => (SDL_PixelFormat)0x15321002U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGBA4444 => (SDL_PixelFormat)0x15421002U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ABGR4444 => (SDL_PixelFormat)0x15721002U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGRA4444 => (SDL_PixelFormat)0x15821002U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ARGB1555 => (SDL_PixelFormat)0x15331002U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGBA5551 => (SDL_PixelFormat)0x15441002U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ABGR1555 => (SDL_PixelFormat)0x15731002U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGRA5551 => (SDL_PixelFormat)0x15841002U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGB565 => (SDL_PixelFormat)0x15151002U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGR565 => (SDL_PixelFormat)0x15551002U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGB24 => (SDL_PixelFormat)0x17101803U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGR24 => (SDL_PixelFormat)0x17401803U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_XRGB8888 => (SDL_PixelFormat)0x16161804U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGBX8888 => (SDL_PixelFormat)0x16261804U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_XBGR8888 => (SDL_PixelFormat)0x16561804U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGRX8888 => (SDL_PixelFormat)0x16661804U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ARGB8888 => (SDL_PixelFormat)0x16362004U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGBA8888 => (SDL_PixelFormat)0x16462004U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ABGR8888 => (SDL_PixelFormat)0x16762004U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGRA8888 => (SDL_PixelFormat)0x16862004U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_XRGB2101010 => (SDL_PixelFormat)0x16172004U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_XBGR2101010 => (SDL_PixelFormat)0x16572004U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ARGB2101010 => (SDL_PixelFormat)0x16372004U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ABGR2101010 => (SDL_PixelFormat)0x16772004U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGB48 => (SDL_PixelFormat)0x18103006U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGR48 => (SDL_PixelFormat)0x18403006U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGBA64 => (SDL_PixelFormat)0x18204008U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ARGB64 => (SDL_PixelFormat)0x18304008U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGRA64 => (SDL_PixelFormat)0x18504008U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ABGR64 => (SDL_PixelFormat)0x18604008U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGB48_FLOAT => (SDL_PixelFormat)0x1A103006U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGR48_FLOAT => (SDL_PixelFormat)0x1A403006U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGBA64_FLOAT => (SDL_PixelFormat)0x1A204008U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ARGB64_FLOAT => (SDL_PixelFormat)0x1A304008U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGRA64_FLOAT => (SDL_PixelFormat)0x1A504008U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ABGR64_FLOAT => (SDL_PixelFormat)0x1A604008U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGB96_FLOAT => (SDL_PixelFormat)0x1B10600CU;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGR96_FLOAT => (SDL_PixelFormat)0x1B40600CU;
    public static SDL_PixelFormat SDL_PIXELFORMAT_RGBA128_FLOAT => (SDL_PixelFormat)0x1B208010U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ARGB128_FLOAT => (SDL_PixelFormat)0x1B308010U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_BGRA128_FLOAT => (SDL_PixelFormat)0x1B508010U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_ABGR128_FLOAT => (SDL_PixelFormat)0x1B608010U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_YV12 => (SDL_PixelFormat)0x32315659U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_IYUV => (SDL_PixelFormat)0x56555949U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_YUY2 => (SDL_PixelFormat)0x32595559U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_UYVY => (SDL_PixelFormat)0x59565955U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_YVYU => (SDL_PixelFormat)0x55595659U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_NV12 => (SDL_PixelFormat)0x3231564EU;
    public static SDL_PixelFormat SDL_PIXELFORMAT_NV21 => (SDL_PixelFormat)0x3132564EU;
    public static SDL_PixelFormat SDL_PIXELFORMAT_P010 => (SDL_PixelFormat)0x30313050U;
    public static SDL_PixelFormat SDL_PIXELFORMAT_EXTERNAL_OES => (SDL_PixelFormat)0x2053454FU;
    public static SDL_PixelFormat SDL_PIXELFORMAT_MJPG => (SDL_PixelFormat)0x47504A4DU;

    public static SDL_PixelFormat SDL_PIXELFORMAT_RGBA32 =>
        BitConverter.IsLittleEndian ? SDL_PIXELFORMAT_ABGR8888 : SDL_PIXELFORMAT_RGBA8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_ARGB32 =>
        BitConverter.IsLittleEndian ? SDL_PIXELFORMAT_BGRA8888 : SDL_PIXELFORMAT_ARGB8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_BGRA32 =>
        BitConverter.IsLittleEndian ? SDL_PIXELFORMAT_ARGB8888 : SDL_PIXELFORMAT_BGRA8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_ABGR32 =>
        BitConverter.IsLittleEndian ? SDL_PIXELFORMAT_RGBA8888 : SDL_PIXELFORMAT_ABGR8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_RGBX32 =>
        BitConverter.IsLittleEndian ? SDL_PIXELFORMAT_XBGR8888 : SDL_PIXELFORMAT_RGBX8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_XRGB32 =>
        BitConverter.IsLittleEndian ? SDL_PIXELFORMAT_BGRX8888 : SDL_PIXELFORMAT_XRGB8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_BGRX32 =>
        BitConverter.IsLittleEndian ? SDL_PIXELFORMAT_XRGB8888 : SDL_PIXELFORMAT_BGRX8888;

    public static SDL_PixelFormat SDL_PIXELFORMAT_XBGR32 =>
        BitConverter.IsLittleEndian ? SDL_PIXELFORMAT_RGBX8888 : SDL_PIXELFORMAT_XBGR8888;

    public enum SDL_Colorspace : uint;

    [SuppressMessage(
        "Style",
        "IDE1006:Naming Styles",
        Justification = "Respect the SDL naming convention"
    )]
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_Color
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;
    }

    [SuppressMessage(
        "Style",
        "IDE1006:Naming Styles",
        Justification = "Respect the SDL naming convention"
    )]
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_FColor
    {
        public float r;
        public float g;
        public float b;
        public float a;
    }

    [SuppressMessage(
        "Style",
        "IDE1006:Naming Styles",
        Justification = "Respect the SDL naming convention"
    )]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_Palette
    {
        public int ncolors;
        public SDL_Color* colors;
        public uint version;
        public int refcount;
    }

    [SuppressMessage(
        "Style",
        "IDE1006:Naming Styles",
        Justification = "Respect the SDL naming convention"
    )]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_PixelFormatDetails
    {
        public SDL_PixelFormat format;
        public byte bits_per_pixel;
        public byte bytes_per_pixel;
        private fixed byte _padding[2];
        public uint Rmask;
        public uint Gmask;
        public uint Bmask;
        public uint Amask;
        public byte Rbits;
        public byte Gbits;
        public byte Bbits;
        public byte Abits;
        public byte Rshift;
        public byte Gshift;
        public byte Bshift;
        public byte Ashift;
    }

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetPixelFormatName),
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(SdlOwnedUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial string SDL_GetPixelFormatName(SDL_PixelFormat format);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetMasksForPixelFormat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_GetMasksForPixelFormat(
        SDL_PixelFormat format,
        out int bpp,
        out uint Rmask,
        out uint Gmask,
        out uint Bmask,
        out uint Amask
    );

    [SuppressMessage(
        "Style",
        "IDE1006:Naming Styles",
        Justification = "Respect the SDL naming convention"
    )]
    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetPixelFormatForMasks))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_PixelFormat SDL_GetPixelFormatForMasks(
        int bpp,
        uint Rmask,
        uint Gmask,
        uint Bmask,
        uint Amask
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetPixelFormatDetails))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_PixelFormatDetails* SDL_GetPixelFormatDetails(
        SDL_PixelFormat format
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CreatePalette))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Palette* SDL_CreatePalette(int ncolors);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetPaletteColors))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetPaletteColors(
        SDL_Palette* palette,
        [In]
        [MarshalUsing(
            typeof(ArrayMarshaller<Color, SDL_Color>),
            CountElementName = nameof(ncolors)
        )]
            Color[] colors,
        int firstColor,
        int ncolors
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_DestroyPalette))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void SDL_DestroyPalette(SDL_Palette* palette);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_MapRGB))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial uint SDL_MapRGB(
        SDL_PixelFormatDetails* format,
        SDL_Palette* palette,
        byte r,
        byte g,
        byte b
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_MapRGBA))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial uint SDL_MapRGBA(
        SDL_PixelFormatDetails* format,
        SDL_Palette* palette,
        byte r,
        byte g,
        byte b,
        byte a
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRGB))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void SDL_GetRGB(
        uint pixel,
        SDL_PixelFormatDetails* format,
        SDL_Palette* palette,
        out byte r,
        out byte g,
        out byte b
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetRGBA))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void SDL_GetRGBA(
        uint pixel,
        SDL_PixelFormatDetails* format,
        SDL_Palette* palette,
        out byte r,
        out byte g,
        out byte b,
        out byte a
    );
}
