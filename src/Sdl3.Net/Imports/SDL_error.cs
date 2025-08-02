using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Sdl3.Net.CustomMarshallers;

namespace Sdl3.Net.Imports;

internal static partial class SDL3
{
    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetError),
        StringMarshalling = StringMarshalling.Custom,
        StringMarshallingCustomType = typeof(SdlOwnedUtf8StringMarshaller)
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial string SDL_GetError();
}
