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

using System.Diagnostics;
using System.Runtime.InteropServices;
using Sdl3.Net.AppSubsystems;
using Sdl3.Net.Options;
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net;

/// <summary>
/// Represents the SDL3 application.
/// </summary>
public class App : IDisposable
{
    private bool _disposedValue;
    private readonly AppOptions _options = new();

    /// <summary>
    /// Manages the application metadata.
    /// </summary>
    public static class Metadata
    {
        /// <summary>
        /// Gets the application metadata name property.
        /// </summary>
        public static string? Name => SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_NAME_STRING);

        /// <summary>
        /// Gets the application metadata version property.
        /// </summary>
        public static Version? Version
        {
            get
            {
                var versionString = SDL_GetAppMetadataProperty(
                    SDL_PROP_APP_METADATA_VERSION_STRING
                );
                return versionString is null ? null : Version.Parse(versionString);
            }
        }

        /// <summary>
        /// Gets the application metadata identifier property.
        /// </summary>
        public static string? Identifier =>
            SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_IDENTIFIER_STRING);

        /// <summary>
        /// Gets the application metadata creator property.
        /// </summary>
        public static string? Creator =>
            SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_CREATOR_STRING);

        /// <summary>
        /// Gets the application metadata copyright property.
        /// </summary>
        public static string? Copyright =>
            SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_COPYRIGHT_STRING);

        /// <summary>
        /// Gets the application metadata URL property.
        /// </summary>
        public static string? Url => SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_URL_STRING);

        /// <summary>
        /// Gets the application metadata type property.
        /// </summary>
        public static string? AppType =>
            SDL_GetAppMetadataProperty(SDL_PROP_APP_METADATA_TYPE_STRING);

        /// <summary>
        /// Sets the application metadata name property.
        /// </summary>
        /// <param name="name">The name to set.</param>
        /// <exception cref="ExternalException">Thrown when the name fails to set.</exception>
        public static void SetName(string? name)
        {
            if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_NAME_STRING, name))
            {
                throw new ExternalException(
                    $"Failed to set app metadata name '{name}': {SDL_GetError()}"
                );
            }
        }

        /// <summary>
        /// Sets the application metadata version property.
        /// </summary>
        /// <param name="version">The version to set.</param>
        /// <exception cref="ExternalException">Thrown when the version fails to set.</exception>
        public static void SetVersion(Version? version)
        {
            if (
                !SDL_SetAppMetadataProperty(
                    SDL_PROP_APP_METADATA_VERSION_STRING,
                    version?.ToString()
                )
            )
            {
                throw new ExternalException(
                    $"Failed to set app metadata version '{version}': {SDL_GetError()}"
                );
            }
        }

        /// <summary>
        /// Sets the application metadata identifier property.
        /// </summary>
        /// <param name="identifier">The identifier to set.</param>
        /// <exception cref="ExternalException">Thrown when the identifier fails to set.</exception>
        public static void SetIdentifier(string? identifier)
        {
            if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_IDENTIFIER_STRING, identifier))
            {
                throw new ExternalException(
                    $"Failed to set app metadata identifier '{identifier}': {SDL_GetError()}"
                );
            }
        }

        /// <summary>
        /// Sets the application metadata creator property.
        /// </summary>
        /// <param name="creator">The creator to set.</param>
        /// <exception cref="ExternalException">Thrown when the creator fails to set.</exception>
        public static void SetCreator(string? creator)
        {
            if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_CREATOR_STRING, creator))
            {
                throw new ExternalException(
                    $"Failed to set app metadata creator '{creator}': {SDL_GetError()}"
                );
            }
        }

        /// <summary>
        /// Sets the application metadata copyright property.
        /// </summary>
        /// <param name="copyright">The copyright to set.</param>
        /// <exception cref="ExternalException">Thrown when the copyright fails to set.</exception>
        public static void SetCopyright(string? copyright)
        {
            if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_COPYRIGHT_STRING, copyright))
            {
                throw new ExternalException(
                    $"Failed to set app metadata copyright '{copyright}': {SDL_GetError()}"
                );
            }
        }

        /// <summary>
        /// Sets the application metadata URL property.
        /// </summary>
        /// <param name="url">The URL to set.</param>
        /// <exception cref="ExternalException">Thrown when the URL fails to set.</exception>
        public static void SetUrl(string? url)
        {
            if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_URL_STRING, url))
            {
                throw new ExternalException(
                    $"Failed to set app metadata URL '{url}': {SDL_GetError()}"
                );
            }
        }

        /// <summary>
        /// Sets the application metadata type property.
        /// </summary>
        /// <param name="type">The type to set.</param>
        /// <exception cref="ExternalException">Thrown when the type fails to set.</exception>
        public static void SetAppType(string? type)
        {
            if (!SDL_SetAppMetadataProperty(SDL_PROP_APP_METADATA_TYPE_STRING, type))
            {
                throw new ExternalException(
                    $"Failed to set app metadata type '{type}': {SDL_GetError()}"
                );
            }
        }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="AudioSubsystem"/>.
    /// </summary>
    /// <exception cref="ExternalException">Thrown when the audio subsystem fails to initialize.</exception>
    public AudioSubsystem Audio => new(this);

    /// <summary>
    /// Creates a new instance of the <see cref="CameraSubsystem"/>.
    /// </summary>
    /// <exception cref="ExternalException">Thrown when the camera subsystem fails to initialize.</exception>
    public CameraSubsystem Camera => new(this);

    /// <summary>
    /// Creates a new instance of the <see cref="GamepadSubsystem"/>.
    /// </summary>
    /// <exception cref="ExternalException">Thrown when the event subsystem fails to initialize.</exception>
    public EventSubsystem Event => new(this);

    /// <summary>
    /// Creates a new instance of the <see cref="SensorSubsystem"/>.
    /// </summary>
    /// <exception cref="ExternalException">Thrown when the gamepad subsystem fails to initialize.</exception>
    public GamepadSubsystem Gamepad => new(this);

    /// <summary>
    /// Creates a new instance of the <see cref="SensorSubsystem"/>.
    /// </summary>
    /// <exception cref="ExternalException">Thrown when the sensor subsystem fails to initialize.</exception>
    public SensorSubsystem Sensor => new(this);

    /// <summary>
    /// Creates a new instance of the <see cref="VideoSubsystem"/>.
    /// </summary>
    /// <exception cref="ExternalException">Thrown when the video subsystem fails to initialize.</exception>
    public VideoSubsystem Video => new(this);

    /// <summary>
    /// Gets the time since the application started.
    /// </summary>
    public TimeSpan TimeSinceStartup
    {
        get
        {
            Debug.Assert(this is not null); // Ensure the app is initialized before accessing this property
            return TimeSpan.FromMilliseconds(SDL_GetTicks());
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    /// <param name="options">An optional action to configure the application options.</param>
    /// <exception cref="ExternalException">Thrown when the application metadata fails to set.</exception>
    public App(Action<AppOptions>? options = null)
    {
        options?.Invoke(_options);

        if (options is not null)
        {
            if (
                !SDL_SetAppMetadata(
                    _options.AppName,
                    _options.AppVersion?.ToString(),
                    _options.AppIdentifier
                )
            )
            {
                throw new ExternalException(
                    $"Failed to set app metadata '{_options}': {SDL_GetError()}"
                );
            }
        }

        if (!SDL_InitSubSystem(new SDL_InitFlags(0)))
        {
            throw new ExternalException($"Failed to initialize SDL3: {SDL_GetError()}");
        }
    }

    /// <summary>
    /// Terminates the SDL3 application and all its subsystems.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // No managed resources to dispose
            }

            SDL_Quit();
            _disposedValue = true;
        }
    }

    /// <summary>
    /// Finalizer for the <see cref="App"/> class.
    /// </summary>
    ~App()
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
