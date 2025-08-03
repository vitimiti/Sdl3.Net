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

namespace Sdl3.Net.AppSubsystems;

/// <summary>
/// Represents the video subsystem of the SDL3 application.
/// </summary>
public class VideoSubsystem : IDisposable
{
    private readonly App _app;
    private bool _disposedValue;

    /// <summary>
    /// Gets a value indicating whether the video subsystem was initialized.
    /// </summary>
    public static bool WasInitialized => SDL_WasInit(SDL_INIT_VIDEO).Value != 0;

    internal VideoSubsystem(App app)
    {
        if (!SDL_InitSubSystem(SDL_INIT_VIDEO))
        {
            throw new InvalidOperationException(
                $"Failed to initialize {nameof(VideoSubsystem)}: {SDL_GetError()}"
            );
        }

        _app = app;
    }

    /// <summary>
    /// Terminates the video subsystem.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // No managed resources to dispose
            }

            SDL_QuitSubSystem(SDL_INIT_VIDEO);
            _disposedValue = true;
        }
    }

    /// <summary>
    /// Finalizer for the <see cref="VideoSubsystem"/> class.
    /// </summary>
    ~VideoSubsystem()
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
}
