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
using Sdl3.Net.PropertiesSystem;
using Sdl3.Net.Shapes;
using Sdl3.Net.StreamManagement;
using Sdl3.Net.Video.Blending;
using Sdl3.Net.Video.Pixels;
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.Video;

/// <summary>
/// Represents a surface in SDL.
/// </summary>
public class Surface : IDisposable, ICloneable
{
    private readonly unsafe SDL_Surface* _handle;
    private readonly bool _ownsHandle;

    private bool _disposedValue;
    private bool _isLocked;

    /// <summary>
    /// Gets whether the surface must be locked before accessing its pixels.
    /// </summary>
    public bool MustLock
    {
        get
        {
            unsafe
            {
                return (_handle->flags & SDL_SURFACE_LOCK_NEEDED) == SDL_SURFACE_LOCK_NEEDED;
            }
        }
    }

    /// <summary>
    /// Gets the pixel format of the surface.
    /// </summary>
    public PixelFormat Format
    {
        get
        {
            unsafe
            {
                return new PixelFormat(_handle->format);
            }
        }
    }

    /// <summary>
    /// Gets the size of the surface.
    /// </summary>
    public (int Width, int Height) Size
    {
        get
        {
            unsafe
            {
                return (_handle->w, _handle->h);
            }
        }
    }

    /// <summary>
    /// Gets the pitch of the surface.
    /// </summary>
    public int Pitch
    {
        get
        {
            unsafe
            {
                return _handle->pitch;
            }
        }
    }

    /// <summary>
    /// Gets the number of references to the surface.
    /// </summary>
    public int RefCount
    {
        get
        {
            unsafe
            {
                return _handle->refcount;
            }
        }
    }

    /// <summary>
    /// Gets the color space of the surface.
    /// </summary>
    public ColorSpace ColorSpace
    {
        get
        {
            unsafe
            {
                return new ColorSpace(SDL_GetSurfaceColorspace(_handle));
            }
        }
    }

    /// <summary>
    /// Whether the surface has alternate images.
    /// </summary>
    public bool HasAlternateImages
    {
        get
        {
            unsafe
            {
                return SDL_SurfaceHasAlternateImages(_handle);
            }
        }
    }

    /// <summary>
    /// Whether the surface has RLE (Run-Length Encoding) enabled.
    /// </summary>
    public bool HasRleEnabled
    {
        get
        {
            unsafe
            {
                return SDL_SurfaceHasRLE(_handle);
            }
        }
    }

    /// <summary>
    /// Whether the surface has a color key.
    /// </summary>
    public bool HasColorKey
    {
        get
        {
            unsafe
            {
                return SDL_SurfaceHasColorKey(_handle);
            }
        }
    }

    private unsafe Surface(SDL_Surface* surface, bool ownsHandle = false)
    {
        _handle = surface;
        _ownsHandle = ownsHandle;
    }

    /// <summary>
    /// Creates a new surface with the specified size and pixel format.
    /// </summary>
    /// <param name="size">The size of the surface.</param>
    /// <param name="format">The pixel format of the surface.</param>
    /// <exception cref="ExternalException">Thrown when the surface creation fails.</exception>
    public Surface((int Width, int Height) size, PixelFormat format)
    {
        unsafe
        {
            _handle = SDL_CreateSurface(size.Width, size.Height, format.Handle);
            if (_handle is null)
            {
                throw new ExternalException(
                    $"Failed to create surface with size '{size}' and format '{format}': {SDL_GetError()}"
                );
            }
        }

        _ownsHandle = true;
    }

    /// <summary>
    /// Creates a new surface with the specified size, pixel format, pixel data, and pitch.
    /// </summary>
    /// <param name="size">The size of the surface.</param>
    /// <param name="format">The pixel format of the surface.</param>
    /// <param name="pixels">The pixel data for the surface.</param>
    /// <param name="pitch">The pitch of the pixel data.</param>
    /// <exception cref="ExternalException">Thrown when the surface creation fails.</exception>
    public Surface(
        (int Width, int Height) size,
        PixelFormat format,
        ReadOnlyMemory<byte> pixels,
        int pitch
    )
    {
        unsafe
        {
            fixed (byte* ptr = pixels.Span)
            {
                _handle = SDL_CreateSurfaceFrom(size.Width, size.Height, format.Handle, ptr, pitch);
                if (_handle is null)
                {
                    throw new ExternalException(
                        $"Failed to create surface with size '{size}', format '{format}', and pixel data with pitch '{pitch}': {SDL_GetError()}"
                    );
                }
            }
        }

        _ownsHandle = true;
    }

    /// <summary>
    /// Terminates the surface and releases its resources.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // No managed resources to dispose
            }

            unsafe
            {
                if (_ownsHandle && _handle is not null)
                {
                    unsafe
                    {
                        SDL_DestroySurface(_handle);
                    }
                }
            }

            if (_isLocked)
            {
                Unlock();
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Releases the resources used by the surface.
    /// </summary>
    ~Surface()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Loads a BMP image from the specified stream.
    /// </summary>
    /// <param name="src">The source stream.</param>
    /// <returns>The loaded surface.</returns>
    /// <exception cref="ExternalException">Thrown when the loading fails.</exception>
    public static Surface LoadBmp(IoStream src)
    {
        unsafe
        {
            var surface = SDL_LoadBMP_IO(src.Handle, closeio: false);
            if (surface is null)
            {
                throw new ExternalException($"Failed to load BMP from stream: {SDL_GetError()}");
            }

            return new Surface(surface);
        }
    }

    /// <summary>
    /// Loads a BMP image from the specified file.
    /// </summary>
    /// <param name="file">The path to the BMP file.</param>
    /// <returns>The loaded surface.</returns>
    /// <exception cref="ExternalException">Thrown when the loading fails.</exception>
    public static Surface LoadBmp(string file)
    {
        unsafe
        {
            var surface = SDL_LoadBMP(file);
            if (surface is null)
            {
                throw new ExternalException(
                    $"Failed to load BMP from file '{file}': {SDL_GetError()}"
                );
            }

            return new Surface(surface);
        }
    }

    /// <summary>
    /// Gets the pixel data of the surface.
    /// </summary>
    /// <returns>A read-only memory containing the pixel data of the surface.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the surface pixels are not available.</exception>
    public ReadOnlyMemory<byte> GetPixels()
    {
        unsafe
        {
            if (_handle->pixels is null)
            {
                throw new InvalidOperationException($"Surface pixels '{this}' are not available.");
            }

            return new Memory<byte>(
                new Span<byte>((byte*)_handle->pixels, _handle->pitch * _handle->h).ToArray()
            );
        }
    }

    /// <summary>
    /// Gets the properties of the surface.
    /// </summary>
    /// <exception cref="ExternalException">Thrown when the properties retrieval fails.</exception>
    public SurfaceProperties GetProperties()
    {
        unsafe
        {
            var props = SDL_GetSurfaceProperties(_handle);
            return props.Value == 0
                ? throw new ExternalException(
                    $"Failed to get surface '{this}' properties: {SDL_GetError()}"
                )
                : new SurfaceProperties(props);
        }
    }

    /// <summary>
    /// Gets the palette of the surface.
    /// </summary>
    /// <returns>
    /// The palette of the surface, or null if the surface does not have a palette.
    /// </returns>
    public Palette? GetPalette()
    {
        unsafe
        {
            var palette = SDL_GetSurfacePalette(_handle);
            return palette is null ? null : new Palette(palette);
        }
    }

    /// <summary>
    /// Gets the alternate images of the surface.
    /// </summary>
    /// <returns>
    /// A read-only collection of alternate images of the surface.
    /// </returns>
    /// <exception cref="ExternalException">Thrown when the alternate images retrieval fails.</exception>
    public IReadOnlyCollection<Surface> GetImages()
    {
        unsafe
        {
            var images = SDL_GetSurfaceImages(_handle, out int count);
            if (images is null)
            {
                throw new ExternalException(
                    $"Failed to get surface '{this}' images: {SDL_GetError()}"
                );
            }

            try
            {
                var result = new Surface[count];
                for (int i = 0; i < count; i++)
                {
                    result[i] = new Surface(images[i]);
                }

                return result.AsReadOnly();
            }
            finally
            {
                unsafe
                {
                    SDL_free(images);
                }
            }
        }
    }

    /// <summary>
    /// Gets the color key for the surface.
    /// </summary>
    /// <returns>The color key for the surface.</returns>
    /// <exception cref="ExternalException">Thrown when the color key retrieval fails.</exception>
    public ColoredPixel GetColorKey()
    {
        unsafe
        {
            if (!SDL_GetSurfaceColorKey(_handle, out var colorKey))
            {
                throw new ExternalException(
                    $"Failed to get color key for surface '{this}': {SDL_GetError()}"
                );
            }

            return new ColoredPixel(colorKey);
        }
    }

    /// <summary>
    /// Gets the color modulation for the surface.
    /// </summary>
    /// <returns>The color modulation for the surface.</returns>
    /// <exception cref="ExternalException">Thrown when the color modulation retrieval fails.</exception>
    public Color GetColorMod()
    {
        unsafe
        {
            if (!SDL_GetSurfaceColorMod(_handle, out var r, out var g, out var b))
            {
                throw new ExternalException(
                    $"Failed to get color mod for surface '{this}': {SDL_GetError()}"
                );
            }

            return new Color(r, g, b);
        }
    }

    /// <summary>
    /// Gets the alpha modulation for the surface.
    /// </summary>
    /// <returns>The alpha modulation for the surface.</returns>
    /// <exception cref="ExternalException">Thrown when the alpha modulation retrieval fails.</exception>
    public byte GetAlphaMod()
    {
        unsafe
        {
            if (!SDL_GetSurfaceAlphaMod(_handle, out var alpha))
            {
                throw new ExternalException(
                    $"Failed to get alpha mod for surface '{this}': {SDL_GetError()}"
                );
            }

            return alpha;
        }
    }

    /// <summary>
    /// Gets the blend mode for the surface.
    /// </summary>
    /// <returns>The blend mode for the surface.</returns>
    /// <exception cref="ExternalException">Thrown when the blend mode retrieval fails.</exception>
    public BlendMode GetBlendMode()
    {
        unsafe
        {
            if (!SDL_GetSurfaceBlendMode(_handle, out var blendMode))
            {
                throw new ExternalException(
                    $"Failed to get blend mode for surface '{this}': {SDL_GetError()}"
                );
            }

            return blendMode;
        }
    }

    /// <summary>
    /// Gets the clip rectangle of the surface.
    /// </summary>
    /// <returns>The clip rectangle of the surface.</returns>
    /// <exception cref="ExternalException">Thrown when the clip rectangle retrieval fails.</exception>
    public Rect GetClipRect()
    {
        unsafe
        {
            if (!SDL_GetSurfaceClipRect(_handle, out var rect))
            {
                throw new ExternalException(
                    $"Failed to get clip rect for surface '{this}': {SDL_GetError()}"
                );
            }

            return rect;
        }
    }

    /// <summary>
    /// Sets the pixels of the surface.
    /// </summary>
    /// <param name="value">The pixel data to set.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the surface must be locked before setting pixels but couldn't automatically lock.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the surface pixels are not available.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when the provided pixel data is invalid.</exception>
    public void SetPixels(Memory<byte> value)
    {
        if (MustLock && !_isLocked)
        {
            try
            {
                Lock();
            }
            catch (ExternalException ex)
            {
                throw new InvalidOperationException(
                    $"Surface '{this}' failed to lock before setting pixels.",
                    ex
                );
            }
        }

        unsafe
        {
            if (_handle->pixels is null)
            {
                throw new InvalidOperationException($"Surface '{this}' pixels are not available.");
            }

            if (value.Length != _handle->pitch * _handle->h)
            {
                throw new ArgumentException(
                    $"The provided pixel data does not match the surface '{this}' pixel size '{_handle->pitch * _handle->h}'."
                );
            }

            fixed (byte* ptr = value.Span)
            {
                _handle->pixels = ptr;
            }
        }
    }

    /// <summary>
    /// Sets the color space of the surface.
    /// </summary>
    /// <param name="colorSpace">The color space to set.</param>
    /// <exception cref="ExternalException">Thrown when the color space setting fails.</exception>
    public void SetColorSpace(ColorSpace colorSpace)
    {
        unsafe
        {
            if (!SDL_SetSurfaceColorspace(_handle, colorSpace.Handle))
            {
                throw new ExternalException(
                    $"Failed to set surface '{this}' colorspace to '{colorSpace}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Sets the palette of the surface.
    /// </summary>
    /// <param name="palette">The palette to set.</param>
    /// <exception cref="ExternalException">Thrown when the palette setting fails.</exception>
    public void SetPalette(Palette palette)
    {
        unsafe
        {
            if (!SDL_SetSurfacePalette(_handle, palette.Handle))
            {
                throw new ExternalException(
                    $"Failed to set surface '{this}' palette to '{palette}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Sets the RLE (Run-Length Encoding) status of the surface.
    /// </summary>
    /// <param name="enabled">Whether to enable or disable RLE.</param>
    /// <exception cref="ExternalException">Thrown when the RLE status setting fails.</exception>
    public void SetRleStatus(bool enabled)
    {
        unsafe
        {
            if (!SDL_SetSurfaceRLE(_handle, enabled))
            {
                throw new ExternalException(
                    $"Failed to set RLE status for surface '{this}' to '{enabled}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Sets the color key for the surface.
    /// </summary>
    /// <param name="enabled">Whether to enable or disable the color key.</param>
    /// <param name="colorKey">The color key to set.</param>
    /// <exception cref="ExternalException">Thrown when the color key setting fails.</exception>
    public void SetColorKey(bool enabled, ColoredPixel colorKey)
    {
        unsafe
        {
            if (!SDL_SetSurfaceColorKey(_handle, enabled, colorKey.Value))
            {
                throw new ExternalException(
                    $"Failed to set color key for surface '{this}' to '{colorKey}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Sets the color modulation for the surface.
    /// </summary>
    /// <param name="color">The color to set for modulation.</param>
    /// <exception cref="ExternalException">Thrown when the color modulation setting fails.</exception>
    public void SetColorMod(Color color)
    {
        unsafe
        {
            if (!SDL_SetSurfaceColorMod(_handle, color.Red, color.Green, color.Blue))
            {
                throw new ExternalException(
                    $"Failed to set color mod for surface '{this}' to color '{color}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Sets the alpha modulation for the surface.
    /// </summary>
    /// <param name="alpha">The alpha value to set.</param>
    /// <exception cref="ExternalException">Thrown when the alpha modulation setting fails.</exception>
    public void SetAlphaMod(byte alpha)
    {
        unsafe
        {
            if (!SDL_SetSurfaceAlphaMod(_handle, alpha))
            {
                throw new ExternalException(
                    $"Failed to set alpha mod for surface '{this}' to '{alpha}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Sets the blend mode for the surface.
    /// </summary>
    /// <param name="mode">The blend mode to set.</param>
    /// <exception cref="ExternalException">Thrown when the blend mode setting fails.</exception>
    public void SetBlendMode(BlendMode mode)
    {
        unsafe
        {
            if (!SDL_SetSurfaceBlendMode(_handle, mode))
            {
                throw new ExternalException(
                    $"Failed to set blend mode for surface '{this}' to '{mode}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Sets the clipping rectangle for the surface.
    /// </summary>
    /// <param name="rect">The clipping rectangle to set, or null to disable clipping.</param>
    /// <exception cref="ExternalException">Thrown when the clipping rectangle setting fails.</exception>
    public void SetClipRect(Rect? rect)
    {
        unsafe
        {
            if (!SDL_SetSurfaceClipRect(_handle, rect))
            {
                throw new ExternalException(
                    $"Failed to set clip rect for surface '{this}' to '{rect}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Creates a palette that belongs to the surface.
    /// </summary>
    /// <remarks>
    /// The palette will be automatically destroyed when the surface is destroyed and
    /// using the dispose pattern on it won't do anything.
    /// </remarks>
    /// <exception cref="ExternalException">Thrown when the palette creation fails.</exception>
    public Palette CreatePalette()
    {
        unsafe
        {
            var palette = SDL_CreateSurfacePalette(_handle);
            if (palette is null)
            {
                throw new ExternalException(
                    $"Failed to create surface '{this}' palette: {SDL_GetError()}"
                );
            }

            return new Palette(palette);
        }
    }

    /// <summary>
    /// Adds an alternate image to the surface.
    /// </summary>
    /// <param name="image">The alternate image to add.</param>
    /// <exception cref="ExternalException">Thrown when the alternate image addition fails.</exception>
    public void AddAlternateImage(Surface image)
    {
        unsafe
        {
            if (!SDL_AddSurfaceAlternateImage(_handle, image._handle))
            {
                throw new ExternalException(
                    $"Failed to add alternate image to surface '{this}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Removes all alternate images from the surface.
    /// </summary>
    public void RemoveAlternateImages()
    {
        unsafe
        {
            if (!SDL_SurfaceHasAlternateImages(_handle))
            {
                throw new ExternalException(
                    $"Failed to remove alternate images from surface '{this}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Locks the surface for pixel access.
    /// </summary>
    /// <exception cref="ExternalException">Thrown when the surface locking fails.</exception>
    public void Lock()
    {
        unsafe
        {
            if (!SDL_LockSurface(_handle))
            {
                throw new ExternalException($"Failed to lock surface '{this}': {SDL_GetError()}");
            }
        }

        _isLocked = true;
    }

    /// <summary>
    /// Unlocks the surface after pixel access.
    /// </summary>
    public void Unlock()
    {
        unsafe
        {
            SDL_UnlockSurface(_handle);
        }

        _isLocked = false;
    }

    /// <summary>
    /// Saves the surface to a BMP I/O stream.
    /// </summary>
    /// <param name="dst">The destination I/O stream.</param>
    /// <exception cref="ExternalException">Thrown when the saving fails.</exception>
    public void SaveBmp(IoStream dst)
    {
        unsafe
        {
            if (!SDL_SaveBMP_IO(_handle, dst.Handle, closeio: false))
            {
                throw new ExternalException(
                    $"Failed to save surface '{this}' to stream: {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Saves the surface to a BMP file.
    /// </summary>
    /// <param name="file">The file path to save the BMP image.</param>
    /// <exception cref="ExternalException">Thrown when the saving fails.</exception>
    public void SaveBmp(string file)
    {
        unsafe
        {
            if (!SDL_SaveBMP(_handle, file))
            {
                throw new ExternalException(
                    $"Failed to save surface '{this}' to file '{file}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Flips the surface using the specified flip mode.
    /// </summary>
    /// <param name="flip">The flip mode to use.</param>
    public void Flip(FlipMode flip)
    {
        unsafe
        {
            if (!SDL_FlipSurface(_handle, flip))
            {
                throw new ExternalException($"Failed to flip surface '{this}': {SDL_GetError()}");
            }
        }
    }

    /// <summary>
    /// Premultiplies the alpha channel of the surface.
    /// </summary>
    /// <param name="inear">Whether to use inear premultiplication.</param>
    /// <exception cref="ExternalException">Thrown when the premultiplication fails.</exception>
    public void PremultiplyAlpha(bool inear = false)
    {
        unsafe
        {
            if (!SDL_PremultiplySurfaceAlpha(_handle, inear))
            {
                throw new ExternalException(
                    $"Failed to premultiply alpha for surface '{this}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Clears the surface with the specified color.
    /// </summary>
    /// <param name="color">The color to clear the surface with.</param>
    public void Clear(FColor color)
    {
        unsafe
        {
            if (!SDL_ClearSurface(_handle, color.Red, color.Green, color.Blue, color.Alpha))
            {
                throw new ExternalException(
                    $"Failed to clear surface '{this}' with color '{color}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Clears the surface with the specified color and optional rectangle.
    /// </summary>
    /// <param name="color">The color to clear the surface with.</param>
    /// <param name="rect">The optional rectangle to clear.</param>
    public void Fill(ColoredPixel color, Rect? rect = null)
    {
        unsafe
        {
            if (!SDL_FillSurfaceRect(_handle, rect, color.Value))
            {
                throw new ExternalException(
                    $"Failed to fill surface '{this}' with color '{color}' and rect '{rect}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Fills the surface with the specified color and a collection of rectangles.
    /// </summary>
    /// <param name="color">The color to fill the surface with.</param>
    /// <param name="rects">The collection of rectangles to fill.</param>
    public void Fill(ColoredPixel color, IReadOnlyCollection<Rect> rects)
    {
        unsafe
        {
            if (!SDL_FillSurfaceRects(_handle, rects.ToArray(), rects.Count, color.Value))
            {
                throw new ExternalException(
                    $"Failed to fill surface '{this}' with color '{color}' and rects: {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Blits the surface onto another surface with optional source and destination rectangles.
    /// </summary>
    /// <param name="srcRect">The optional source rectangle.</param>
    /// <param name="dst">The destination surface.</param>
    /// <param name="dstRect">The optional destination rectangle.</param>
    /// <exception cref="ExternalException">Thrown when the blitting operation fails.</exception>
    public void Blit(Rect? srcRect, Surface dst, Rect? dstRect)
    {
        unsafe
        {
            if (!SDL_BlitSurface(_handle, srcRect, dst._handle, dstRect))
            {
                throw new ExternalException(
                    $"Failed to blit surface '{this}' to destination '{dst}' with source rect '{srcRect}' and destination rect '{dstRect}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Blits the surface onto another surface with optional source and destination rectangles,
    /// and a scale mode.
    /// </summary>
    /// <param name="srcRect">The optional source rectangle.</param>
    /// <param name="dst">The destination surface.</param>
    /// <param name="dstRect">The optional destination rectangle.</param>
    /// <param name="scaleMode">The scale mode to use.</param>
    /// <exception cref="ExternalException">Thrown when the blitting operation fails.</exception>
    public void Blit(Rect? srcRect, Surface dst, Rect? dstRect, ScaleMode scaleMode)
    {
        unsafe
        {
            if (!SDL_BlitSurfaceScaled(_handle, srcRect, dst._handle, dstRect, scaleMode))
            {
                throw new ExternalException(
                    $"Failed to blit surface '{this}' to destination '{dst}' with source rect '{srcRect}', destination rect '{dstRect}', and scale mode '{scaleMode}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Blits the surface onto another surface in a tiled manner.
    /// </summary>
    /// <param name="srcRect">The optional source rectangle.</param>
    /// <param name="dst">The destination surface.</param>
    /// <param name="dstRect">The optional destination rectangle.</param>
    /// <exception cref="ExternalException">Thrown when the blitting operation fails.</exception>
    public void BlitTiled(Rect? srcRect, Surface dst, Rect? dstRect)
    {
        unsafe
        {
            if (!SDL_BlitSurfaceTiled(_handle, srcRect, dst._handle, dstRect))
            {
                throw new ExternalException(
                    $"Failed to blit tiled surface '{this}' to destination '{dst}' with source rect '{srcRect}' and destination rect '{dstRect}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Blits the surface onto another surface in a tiled manner with a scale mode.
    /// </summary>
    /// <param name="srcRect">The optional source rectangle.</param>
    /// <param name="dst">The destination surface.</param>
    /// <param name="dstRect">The optional destination rectangle.</param>
    /// <param name="scale">The scale factor.</param>
    /// <param name="scaleMode">The scale mode to use.</param>
    /// <exception cref="ExternalException">Thrown when the blitting operation fails.</exception>
    public void BlitTiled(
        Rect? srcRect,
        Surface dst,
        Rect? dstRect,
        float scale,
        ScaleMode scaleMode
    )
    {
        var actualScale = float.Clamp(scale, 0F, 1F);

        unsafe
        {
            if (
                !SDL_BlitSurfaceTiledWithScale(
                    _handle,
                    srcRect,
                    actualScale,
                    scaleMode,
                    dst._handle,
                    dstRect
                )
            )
            {
                throw new ExternalException(
                    $"Failed to blit tiled surface '{this}' to destination '{dst}' with source rect '{srcRect}', destination rect '{dstRect}', scale '{actualScale}', and scale mode '{scaleMode}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Blits the surface onto another surface using a 9-grid blitting technique.
    /// </summary>
    /// <param name="srcRect">The optional source rectangle.</param>
    /// <param name="dst">The destination surface.</param>
    /// <param name="dstRect">The optional destination rectangle.</param>
    /// <param name="srcRectCornerWidth">The corner widths for the source rectangle.</param>
    /// <param name="scale">The scale factor.</param>
    /// <param name="scaleMode">The scale mode to use.</param>
    /// <exception cref="ExternalException">Thrown when the blitting operation fails.</exception>
    public void Blit9Grid(
        Rect? srcRect,
        Surface dst,
        Rect? dstRect,
        (int Left, int Right, int Top, int Bottom) srcRectCornerWidth,
        float scale,
        ScaleMode scaleMode
    )
    {
        unsafe
        {
            if (
                !SDL_BlitSurface9Grid(
                    _handle,
                    srcRect,
                    srcRectCornerWidth.Left,
                    srcRectCornerWidth.Right,
                    srcRectCornerWidth.Top,
                    srcRectCornerWidth.Bottom,
                    scale,
                    scaleMode,
                    dst._handle,
                    dstRect
                )
            )
            {
                throw new ExternalException(
                    $"Failed to blit 9-grid surface '{this}' to destination '{dst}' with source rect '{srcRect}', destination rect '{dstRect}', corner widths '{srcRectCornerWidth}', scale '{scale}', and scale mode '{scaleMode}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Blits the surface onto another surface without checking for errors.
    /// </summary>
    /// <param name="srcRect">The optional source rectangle.</param>
    /// <param name="dst">The destination surface.</param>
    /// <param name="dstRect">The optional destination rectangle.</param>
    /// <exception cref="ExternalException">Thrown when the blitting operation fails anyway.</exception>
    /// <remarks>
    /// This is a low-level operation and should be used with caution.
    /// </remarks>
    public void BlitUnchecked(Rect? srcRect, Surface dst, Rect? dstRect)
    {
        unsafe
        {
            if (!SDL_BlitSurfaceUnchecked(_handle, srcRect, dst._handle, dstRect))
            {
                throw new ExternalException(
                    $"Failed to blit surface '{this}' to destination '{dst}' with source rect '{srcRect}' and destination rect '{dstRect}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Blits the surface onto another surface without checking for errors,
    /// and a scale mode.
    /// </summary>
    /// <param name="srcRect">The optional source rectangle.</param>
    /// <param name="dst">The destination surface.</param>
    /// <param name="dstRect">The optional destination rectangle.</param>
    /// <param name="scaleMode">The scale mode to use.</param>
    /// <exception cref="ExternalException">Thrown when the blitting operation fails.</exception>
    /// <remarks>
    /// This is a low-level operation and should be used with caution.
    /// </remarks>
    public void BlitUnchecked(Rect? srcRect, Surface dst, Rect? dstRect, ScaleMode scaleMode)
    {
        unsafe
        {
            if (!SDL_BlitSurfaceUncheckedScaled(_handle, srcRect, dst._handle, dstRect, scaleMode))
            {
                throw new ExternalException(
                    $"Failed to blit surface '{this}' to destination '{dst}' with source rect '{srcRect}', destination rect '{dstRect}', and scale mode '{scaleMode}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Stretches the surface to fit the specified destination rectangle using the given scale mode.
    /// </summary>
    /// <param name="srcRect">The optional source rectangle.</param>
    /// <param name="dst">The destination surface.</param>
    /// <param name="dstRect">The optional destination rectangle.</param>
    /// <param name="scaleMode">The scale mode to use.</param>
    /// <exception cref="ExternalException">Thrown when the stretching operation fails.</exception>
    public void Stretch(Rect? srcRect, Surface dst, Rect? dstRect, ScaleMode scaleMode)
    {
        unsafe
        {
            if (!SDL_StretchSurface(_handle, srcRect, dst._handle, dstRect, scaleMode))
            {
                throw new ExternalException(
                    $"Failed to stretch surface '{this}' to destination '{dst}' with source rect '{srcRect}', destination rect '{dstRect}', and scale mode '{scaleMode}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Maps a color to a pixel value in the surface's pixel format.
    /// </summary>
    /// <param name="color">The color to map.</param>
    public ColoredPixel MapRgb(Color color)
    {
        unsafe
        {
            var pixel = SDL_MapSurfaceRGB(_handle, color.Red, color.Green, color.Blue);
            return new ColoredPixel(pixel);
        }
    }

    /// <summary>
    /// Maps a color with alpha to a pixel value in the surface's pixel format.
    /// </summary>
    /// <param name="color">The color to map.</param>
    public ColoredPixel MapRgba(Color color)
    {
        unsafe
        {
            var pixel = SDL_MapSurfaceRGBA(
                _handle,
                color.Red,
                color.Green,
                color.Blue,
                color.Alpha
            );

            return new ColoredPixel(pixel);
        }
    }

    /// <summary>
    /// Reads a pixel from the surface at the specified point.
    /// </summary>
    /// <param name="point">The point to read the pixel from.</param>
    /// <returns>The color of the pixel at the specified point.</returns>
    /// <exception cref="ExternalException">Thrown when the reading operation fails.</exception>
    public Color ReadPixel(Point point)
    {
        unsafe
        {
            if (
                !SDL_ReadSurfacePixel(
                    _handle,
                    point.X,
                    point.Y,
                    out byte r,
                    out byte g,
                    out byte b,
                    out byte a
                )
            )
            {
                throw new ExternalException(
                    $"Failed to read pixel at '{point}' from surface '{this}': {SDL_GetError()}"
                );
            }

            return new Color(r, g, b, a);
        }
    }

    /// <summary>
    /// Reads a pixel from the surface at the specified point as a floating-point color.
    /// </summary>
    /// <param name="point">The point to read the pixel from.</param>
    /// <returns>The color of the pixel at the specified point.</returns>
    /// <exception cref="ExternalException">Thrown when the reading operation fails.</exception>
    public FColor ReadPixelFloat(Point point)
    {
        unsafe
        {
            if (
                !SDL_ReadSurfacePixelFloat(
                    _handle,
                    point.X,
                    point.Y,
                    out float r,
                    out float g,
                    out float b,
                    out float a
                )
            )
            {
                throw new ExternalException(
                    $"Failed to read pixel at '{point}' from surface '{this}': {SDL_GetError()}"
                );
            }

            return new FColor(r, g, b, a);
        }
    }

    /// <summary>
    /// Writes a pixel to the surface at the specified point with the given color.
    /// </summary>
    /// <param name="point">The point to write the pixel to.</param>
    /// <param name="color">The color of the pixel.</param>
    /// /// <exception cref="ExternalException">Thrown when the writing operation fails.</exception>
    public void WritePixel(Point point, Color color)
    {
        unsafe
        {
            if (
                !SDL_WriteSurfacePixel(
                    _handle,
                    point.X,
                    point.Y,
                    color.Red,
                    color.Green,
                    color.Blue,
                    color.Alpha
                )
            )
            {
                throw new ExternalException(
                    $"Failed to write pixel at '{point}' with color '{color}' to surface '{this}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Writes a pixel to the surface at the specified point with the given floating-point color.
    /// </summary>
    /// <param name="point">The point to write the pixel to.</param>
    /// <param name="color">The color of the pixel.</param>
    /// <exception cref="ExternalException">Thrown when the writing operation fails.</exception>
    public void WritePixel(Point point, FColor color)
    {
        unsafe
        {
            if (
                !SDL_WriteSurfacePixelFloat(
                    _handle,
                    point.X,
                    point.Y,
                    color.Red,
                    color.Green,
                    color.Blue,
                    color.Alpha
                )
            )
            {
                throw new ExternalException(
                    $"Failed to write pixel at '{point}' with color '{color}' to surface '{this}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Scales the surface to the specified size using the given scale mode.
    /// </summary>
    /// <param name="size">The target size for the scaled surface.</param>
    /// <param name="scaleMode">The scale mode to use.</param>
    /// <returns>A new surface representing the scaled version.</returns>
    /// <exception cref="ExternalException">Thrown when the scaling operation fails.</exception>
    public Surface Scale((int Width, int Height) size, ScaleMode scaleMode)
    {
        unsafe
        {
            var scaledSurface = SDL_ScaleSurface(_handle, size.Width, size.Height, scaleMode);
            if (scaledSurface is null)
            {
                throw new ExternalException(
                    $"Failed to scale surface '{this}' to size '{size}' with mode '{scaleMode}': {SDL_GetError()}"
                );
            }

            return new Surface(scaledSurface, ownsHandle: true);
        }
    }

    /// <summary>
    /// Converts the surface to a different pixel format.
    /// </summary>
    /// <param name="format">The target pixel format.</param>
    /// <returns>A new surface representing the converted version.</returns>
    /// <exception cref="ExternalException">Thrown when the conversion operation fails.</exception>
    public Surface Convert(PixelFormat format)
    {
        unsafe
        {
            var convertedSurface = SDL_ConvertSurface(_handle, format.Handle);
            if (convertedSurface is null)
            {
                throw new ExternalException(
                    $"Failed to convert surface '{this}' to format '{format}': {SDL_GetError()}"
                );
            }

            return new Surface(convertedSurface, ownsHandle: true);
        }
    }

    /// <summary>
    /// Converts the surface to a different pixel format and color space.
    /// </summary>
    /// <param name="format">The target pixel format.</param>
    /// <param name="colorSpace">The target color space.</param>
    /// <param name="props">The properties to apply during conversion.</param>
    /// <param name="palette">The optional color palette to use.</param>
    /// <returns>A new surface representing the converted version.</returns>
    /// <exception cref="ExternalException">Thrown when the conversion operation fails.</exception>
    public Surface Convert(
        PixelFormat format,
        ColorSpace colorSpace,
        Properties props,
        Palette? palette = null
    )
    {
        unsafe
        {
            var convertedSurface = SDL_ConvertSurfaceAndColorspace(
                _handle,
                format.Handle,
                palette is null ? null : palette.Handle,
                colorSpace.Handle,
                props.Id
            );

            if (convertedSurface is null)
            {
                throw new ExternalException(
                    $"Failed to convert surface '{this}' to format '{format}', colorspace '{colorSpace}', properties '{props}' and palette '{palette}': {SDL_GetError()}"
                );
            }

            return new Surface(convertedSurface, ownsHandle: true);
        }
    }

    /// <summary>
    /// Clones the surface, creating a new instance with the same properties and pixel data.
    /// </summary>
    public object Clone()
    {
        unsafe
        {
            var clonedSurface = SDL_DuplicateSurface(_handle);
            if (clonedSurface is null)
            {
                throw new ExternalException($"Failed to clone surface '{this}': {SDL_GetError()}");
            }

            return new Surface(clonedSurface, ownsHandle: true);
        }
    }
}
