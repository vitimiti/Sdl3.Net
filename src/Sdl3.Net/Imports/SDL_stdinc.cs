using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Sdl3.Net.Imports;

internal static partial class SDL3
{
    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_free))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void SDL_free(void* ptr);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_strdup))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial byte* SDL_strdup(byte* str);
}
