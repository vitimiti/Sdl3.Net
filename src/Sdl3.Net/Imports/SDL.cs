using System.Runtime.InteropServices;

namespace Sdl3.Net.Imports;

internal static partial class SDL3
{
    private static KeyValuePair<string, bool>[] LibraryNames =>
        [
            new("SDL3.dll", OperatingSystem.IsWindows()),
            new("libSDL3.so", OperatingSystem.IsLinux()),
        ];

    static SDL3() =>
        NativeLibrary.SetDllImportResolver(
            typeof(SDL3).Assembly,
            (name, assembly, searchPath) =>
                NativeLibrary.Load(
                    name switch
                    {
                        nameof(SDL3) => LibraryNames.FirstOrDefault(x => x.Value).Key ?? name,
                        _ => name,
                    },
                    assembly,
                    searchPath
                )
        );
}
