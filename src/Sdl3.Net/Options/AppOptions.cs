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

namespace Sdl3.Net.Options;

/// <summary>
/// Represents the options for configuring the SDL3 application.
/// </summary>
public class AppOptions
{
    /// <summary>
    /// Gets or sets the name of the application.
    /// </summary>
    public string? AppName { get; set; }

    /// <summary>
    /// Gets or sets the version of the application.
    /// </summary>
    public Version? AppVersion { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the application.
    /// </summary>
    public string? AppIdentifier { get; set; }

    /// <summary>
    /// Returns a string representation of the application options.
    /// </summary>
    public override string ToString() =>
        $"{nameof(AppOptions)}({nameof(AppName)}: {AppName}, {nameof(AppVersion)}: {AppVersion}, {nameof(AppIdentifier)}: {AppIdentifier})";
}
