using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.PropertiesSystem;

public sealed class IoStreamProperties : Properties
{
    public nint WindowsHandlePointer =>
        GetPointerProperty(SDL_PROP_IOSTREAM_WINDOWS_HANDLE_POINTER);

    public nint StdioFilePointer => GetPointerProperty(SDL_PROP_IOSTREAM_STDIO_FILE_POINTER);

    public long FileDescriptor => GetNumberProperty(SDL_PROP_IOSTREAM_FILE_DESCRIPTOR_NUMBER);

    public nint AndroidAAssetPointer =>
        GetPointerProperty(SDL_PROP_IOSTREAM_ANDROID_AASSET_POINTER);

    public nint MemoryPointer => GetPointerProperty(SDL_PROP_IOSTREAM_MEMORY_POINTER);

    public long MemorySize => GetNumberProperty(SDL_PROP_IOSTREAM_MEMORY_SIZE_NUMBER);

    public nint DynamicMemoryPointer =>
        GetPointerProperty(SDL_PROP_IOSTREAM_DYNAMIC_MEMORY_POINTER);

    public long DynamicChunkSize => GetNumberProperty(SDL_PROP_IOSTREAM_DYNAMIC_CHUNKSIZE_NUMBER);

    internal IoStreamProperties(SDL_PropertiesID propertiesId)
        : base(propertiesId) { }
}
