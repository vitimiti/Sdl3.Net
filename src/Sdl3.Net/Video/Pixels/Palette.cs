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

using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using Sdl3.Net.CustomMarshallers;
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.Video.Pixels;

/// <summary>
/// Represents a palette of colors used in pixel formats.
/// </summary>
public class Palette : IDisposable
{
    private readonly bool _ownsPalette;
    private bool _disposedValue;

    internal unsafe SDL_Palette* Handle { get; }

    /// <summary>
    /// Gets the number of colors in the palette.
    /// </summary>
    public int NumColors
    {
        get
        {
            unsafe
            {
                return Handle->ncolors;
            }
        }
    }

    /// <summary>
    /// Gets the colors in the palette.
    /// </summary>
    public ReadOnlyCollection<Color> Colors
    {
        get
        {
            unsafe
            {
                var colors = new Color[NumColors];
                for (int i = 0; i < NumColors; i++)
                {
                    colors[i] = ColorMarshaller.ConvertToManaged(Handle->colors[i]);
                }

                return new ReadOnlyCollection<Color>(colors);
            }
        }
    }

    /// <summary>
    /// Gets the version of the palette.
    /// </summary>
    public uint Version
    {
        get
        {
            unsafe
            {
                return Handle->version;
            }
        }
    }

    /// <summary>
    /// Gets the reference count of the palette.
    /// </summary>
    public int RefCount
    {
        get
        {
            unsafe
            {
                return Handle->refcount;
            }
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Palette"/> class with a number of colors.
    /// </summary>
    /// /// <param name="numColors">The number of colors in the palette.</param>
    /// <exception cref="ExternalException">Thrown when the palette cannot be created.</exception>
    public Palette(int numColors)
    {
        unsafe
        {
            Handle = SDL_CreatePalette(numColors);
            if (Handle is null)
            {
                throw new ExternalException($"Failed to create palette: {SDL_GetError()}");
            }
        }

        _ownsPalette = true;
    }

    /// <summary>
    /// Terminates the palette and releases its resources.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // No managed resources to dispose
            }

            if (_ownsPalette)
            {
                unsafe
                {
                    SDL_DestroyPalette(Handle);
                }
            }

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Releases the resources used by the palette.
    /// </summary>
    ~Palette()
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
    /// Sets the colors in the palette.
    /// </summary>
    /// <param name="colors">The colors to set in the palette.</param>
    /// <param name="firstColor">The index of the first color to set.</param>
    /// <param name="numColors">The number of colors to set.</param>
    /// <exception cref="ExternalException">Thrown when the colors cannot be set.</exception>
    public void SetColors(IReadOnlyCollection<Color> colors, int firstColor, int numColors)
    {
        unsafe
        {
            if (!SDL_SetPaletteColors(Handle, [.. colors], firstColor, numColors))
            {
                throw new ExternalException(
                    $"Failed to set palette colors [{string.Join(",", colors)}]: {SDL_GetError()}"
                );
            }
        }
    }
}
