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
using Sdl3.Net.Shapes;
using Sdl3.Net.Video;
using Sdl3.Net.Video.Blending;
using Sdl3.Net.Video.Pixels;

namespace Sdl3.Net.Imports;

internal static partial class SDL3
{
    public record struct SDL_SurfaceFlags(uint Value)
    {
        public static SDL_SurfaceFlags operator |(SDL_SurfaceFlags left, SDL_SurfaceFlags right) =>
            new(left.Value | right.Value);

        public static SDL_SurfaceFlags operator &(SDL_SurfaceFlags left, SDL_SurfaceFlags right) =>
            new(left.Value & right.Value);

        public static SDL_SurfaceFlags operator ^(SDL_SurfaceFlags left, SDL_SurfaceFlags right) =>
            new(left.Value ^ right.Value);

        public static SDL_SurfaceFlags operator ~(SDL_SurfaceFlags flags) => new(~flags.Value);

        public static SDL_SurfaceFlags operator <<(SDL_SurfaceFlags flags, int shift) =>
            new(flags.Value << shift);

        public static SDL_SurfaceFlags operator >>(SDL_SurfaceFlags flags, int shift) =>
            new(flags.Value >> shift);
    }

    public static SDL_SurfaceFlags SDL_SURFACE_LOCK_NEEDED => new(0x00000002U);

    [SuppressMessage(
        "Style",
        "IDE1006:Naming Styles",
        Justification = "Respect the SDL naming convention"
    )]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct SDL_Surface
    {
        public SDL_SurfaceFlags flags;
        public SDL_PixelFormat format;
        public int w;
        public int h;
        public int pitch;
        public void* pixels;
        public int refcount;
        public void* reserved;
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CreateSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Surface* SDL_CreateSurface(
        int width,
        int height,
        SDL_PixelFormat format
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CreateSurfaceFrom))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Surface* SDL_CreateSurfaceFrom(
        int width,
        int height,
        SDL_PixelFormat format,
        void* pixels,
        int pitch
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_DestroySurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void SDL_DestroySurface(SDL_Surface* surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_PropertiesID SDL_GetSurfaceProperties(SDL_Surface* surface);

    public const string SDL_PROP_SURFACE_SDR_WHITE_POINT_FLOAT = "SDL.surface.SDR_white_point";
    public const string SDL_PROP_SURFACE_HDR_HEADROOM_FLOAT = "SDL.surface.HDR_headroom";
    public const string SDL_PROP_SURFACE_TONEMAP_OPERATOR_STRING = "SDL.surface.tonemap";
    public const string SDL_PROP_SURFACE_HOTSPOT_X_NUMBER = "SDL.surface.hotspot.x";
    public const string SDL_PROP_SURFACE_HOTSPOT_Y_NUMBER = "SDL.surface.hotspot.y";

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceColorspace))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetSurfaceColorspace(
        SDL_Surface* surface,
        SDL_Colorspace colorspace
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceColorspace))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Colorspace SDL_GetSurfaceColorspace(SDL_Surface* surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CreateSurfacePalette))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Palette* SDL_CreateSurfacePalette(SDL_Surface* surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfacePalette))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetSurfacePalette(
        SDL_Surface* surface,
        SDL_Palette* palette
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfacePalette))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Palette* SDL_GetSurfacePalette(SDL_Surface* surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_AddSurfaceAlternateImage))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_AddSurfaceAlternateImage(
        SDL_Surface* surface,
        SDL_Surface* image
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SurfaceHasAlternateImages))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SurfaceHasAlternateImages(SDL_Surface* surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceImages))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Surface** SDL_GetSurfaceImages(
        SDL_Surface* surface,
        out int count
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_RemoveSurfaceAlternateImages))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void SDL_RemoveSurfaceAlternateImages(SDL_Surface* surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_LockSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_LockSurface(SDL_Surface* surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_UnlockSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial void SDL_UnlockSurface(SDL_Surface* surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_LoadBMP_IO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Surface* SDL_LoadBMP_IO(
        SDL_IOStream src,
        [MarshalAs(UnmanagedType.U1)] bool closeio
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_LoadBMP),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Surface* SDL_LoadBMP(string file);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SaveBMP_IO))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SaveBMP_IO(
        SDL_Surface* surface,
        SDL_IOStream dst,
        [MarshalAs(UnmanagedType.U1)] bool closeio
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SaveBMP),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SaveBMP(SDL_Surface* surface, string file);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceRLE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetSurfaceRLE(
        SDL_Surface* surface,
        [MarshalAs(UnmanagedType.U1)] bool enabled
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SurfaceHasRLE))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SurfaceHasRLE(SDL_Surface* surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceColorKey))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetSurfaceColorKey(
        SDL_Surface* surface,
        [MarshalAs(UnmanagedType.U1)] bool enabled,
        uint key
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SurfaceHasColorKey))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SurfaceHasColorKey(SDL_Surface* surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceColorKey))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_GetSurfaceColorKey(SDL_Surface* surface, out uint key);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceColorMod))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetSurfaceColorMod(
        SDL_Surface* surface,
        byte r,
        byte g,
        byte b
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceColorMod))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_GetSurfaceColorMod(
        SDL_Surface* surface,
        out byte r,
        out byte g,
        out byte b
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceAlphaMod))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetSurfaceAlphaMod(SDL_Surface* surface, byte alpha);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceAlphaMod))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_GetSurfaceAlphaMod(SDL_Surface* surface, out byte alpha);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceBlendMode))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetSurfaceBlendMode(
        SDL_Surface* surface,
        BlendMode blendMode
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceBlendMode))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_GetSurfaceBlendMode(
        SDL_Surface* surface,
        out BlendMode blendMode
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_SetSurfaceClipRect))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetSurfaceClipRect(SDL_Surface* surface, Rect? rect);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetSurfaceClipRect))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_GetSurfaceClipRect(SDL_Surface* surface, out Rect rect);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_FlipSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_FlipSurface(SDL_Surface* surface, FlipMode flip);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_DuplicateSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Surface* SDL_DuplicateSurface(SDL_Surface* surface);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ScaleSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Surface* SDL_ScaleSurface(
        SDL_Surface* surface,
        int width,
        int height,
        ScaleMode scaleMode
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ConvertSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Surface* SDL_ConvertSurface(
        SDL_Surface* surface,
        SDL_PixelFormat format
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ConvertSurfaceAndColorspace))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial SDL_Surface* SDL_ConvertSurfaceAndColorspace(
        SDL_Surface* surface,
        SDL_PixelFormat format,
        SDL_Palette* palette,
        SDL_Colorspace colorspace,
        SDL_PropertiesID properties
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ConvertPixels))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_ConvertPixels(
        int width,
        int height,
        SDL_PixelFormat src_format,
        void* src,
        int src_pitch,
        SDL_PixelFormat dst_format,
        void* dst,
        int dst_pitch
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ConvertPixelsAndColorspace))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_ConvertPixelsAndColorspace(
        int width,
        int height,
        SDL_PixelFormat src_format,
        SDL_Colorspace src_colorspace,
        SDL_PropertiesID src_properties,
        void* src,
        int src_pitch,
        SDL_PixelFormat dst_format,
        SDL_Colorspace ddst_colorspace,
        SDL_PropertiesID dst_properties,
        void* dst,
        int dst_pitch
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_PremultiplyAlpha))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_PremultiplyAlpha(
        int width,
        int height,
        SDL_PixelFormat format,
        void* src,
        int src_pitch,
        SDL_PixelFormat dst_format,
        void* dst,
        int dst_pitch,
        [MarshalAs(UnmanagedType.U1)] bool linear
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_PremultiplySurfaceAlpha))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_PremultiplySurfaceAlpha(
        SDL_Surface* surface,
        [MarshalAs(UnmanagedType.U1)] bool linear
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ClearSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_ClearSurface(
        SDL_Surface* surface,
        float r,
        float g,
        float b,
        float a
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_FillSurfaceRect))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_FillSurfaceRect(SDL_Surface* dst, Rect? rect, uint color);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_FillSurfaceRects))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_FillSurfaceRects(
        SDL_Surface* dst,
        [In]
        [MarshalUsing(typeof(ArrayMarshaller<Rect, SDL_Rect>), CountElementName = nameof(count))]
            Rect[] rects,
        int count,
        uint color
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_BlitSurface(
        SDL_Surface* src,
        Rect? srcrect,
        SDL_Surface* dst,
        Rect? dstrect
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurfaceUnchecked))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_BlitSurfaceUnchecked(
        SDL_Surface* src,
        Rect? srcrect,
        SDL_Surface* dst,
        Rect? dstrect
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurfaceScaled))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_BlitSurfaceScaled(
        SDL_Surface* src,
        Rect? srcrect,
        SDL_Surface* dst,
        Rect? dstrect,
        ScaleMode scaleMode
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurfaceUncheckedScaled))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_BlitSurfaceUncheckedScaled(
        SDL_Surface* src,
        Rect? srcrect,
        SDL_Surface* dst,
        Rect? dstrect,
        ScaleMode scaleMode
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_StretchSurface))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_StretchSurface(
        SDL_Surface* src,
        Rect? srcrect,
        SDL_Surface* dst,
        Rect? dstrect,
        ScaleMode scaleMode
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurfaceTiled))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_BlitSurfaceTiled(
        SDL_Surface* src,
        Rect? srcrect,
        SDL_Surface* dst,
        Rect? dstrect
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurfaceTiledWithScale))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_BlitSurfaceTiledWithScale(
        SDL_Surface* src,
        Rect? srcrect,
        float scale,
        ScaleMode scaleMode,
        SDL_Surface* dst,
        Rect? dstrect
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_BlitSurface9Grid))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_BlitSurface9Grid(
        SDL_Surface* src,
        Rect? srcrect,
        int left_width,
        int right_width,
        int top_height,
        int bottom_height,
        float scale,
        ScaleMode scaleMode,
        SDL_Surface* dst,
        Rect? dstrect
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_MapSurfaceRGB))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial uint SDL_MapSurfaceRGB(
        SDL_Surface* surface,
        byte r,
        byte g,
        byte b
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_MapSurfaceRGBA))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial uint SDL_MapSurfaceRGBA(
        SDL_Surface* surface,
        byte r,
        byte g,
        byte b,
        byte a
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadSurfacePixel))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_ReadSurfacePixel(
        SDL_Surface* surface,
        int x,
        int y,
        out byte r,
        out byte g,
        out byte b,
        out byte a
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_ReadSurfacePixelFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_ReadSurfacePixelFloat(
        SDL_Surface* surface,
        int x,
        int y,
        out float r,
        out float g,
        out float b,
        out float a
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteSurfacePixel))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_WriteSurfacePixel(
        SDL_Surface* surface,
        int x,
        int y,
        byte r,
        byte g,
        byte b,
        byte a
    );

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_WriteSurfacePixelFloat))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_WriteSurfacePixelFloat(
        SDL_Surface* surface,
        int x,
        int y,
        float r,
        float g,
        float b,
        float a
    );
}
