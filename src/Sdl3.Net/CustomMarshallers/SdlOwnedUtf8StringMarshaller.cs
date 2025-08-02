using System.Runtime.InteropServices.Marshalling;
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.CustomMarshallers;

[CustomMarshaller(typeof(string), MarshalMode.ManagedToUnmanagedOut, typeof(ManagedToUnmanagedOut))]
internal static class SdlOwnedUtf8StringMarshaller
{
    public unsafe ref struct ManagedToUnmanagedOut
    {
        private byte* _unmanaged;
        private string? _managed;

        public void FromUnmanaged(byte* unmanaged)
        {
            if (unmanaged is null)
            {
                _managed = null;
                _unmanaged = null;
                return;
            }

            _unmanaged = SDL_strdup(unmanaged);
            _managed = Utf8StringMarshaller.ConvertToManaged(_unmanaged);
        }

        public readonly string? ToManaged() => _managed;

        public void Free()
        {
            if (_unmanaged is null)
            {
                return;
            }

            SDL_free(_unmanaged);
            _unmanaged = null;
        }
    }
}
