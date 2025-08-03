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
using System.Runtime.InteropServices.Marshalling;
using Sdl3.Net.Shapes;
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.CustomMarshallers;

[CustomMarshaller(typeof(Rect), MarshalMode.Default, typeof(RectMarshaller))]
[CustomMarshaller(typeof(Rect), MarshalMode.ManagedToUnmanagedIn, typeof(ManagedToUnmanagedIn))]
[CustomMarshaller(typeof(Rect), MarshalMode.ElementIn, typeof(ElementIn))]
internal static class RectMarshaller
{
    public static Rect ConvertToManaged(SDL_Rect rect) => new(rect.x, rect.y, rect.w, rect.h);

    public static SDL_Rect ConvertToUnmanaged(Rect rect) =>
        new()
        {
            x = rect.X,
            y = rect.Y,
            w = rect.Width,
            h = rect.Height,
        };

    public unsafe ref struct ManagedToUnmanagedIn
    {
        private SDL_Rect* _unmanaged;
        private GCHandle _handle;

        public void FromManaged(Rect? managed)
        {
            if (managed is null)
            {
                _unmanaged = null;
                return;
            }

            SDL_Rect rect = new()
            {
                x = managed.X,
                y = managed.Y,
                w = managed.Width,
                h = managed.Height,
            };

            _handle = GCHandle.Alloc(rect, GCHandleType.Pinned);
            _unmanaged = (SDL_Rect*)_handle.AddrOfPinnedObject();
        }

        public readonly SDL_Rect* ToUnmanaged() => _unmanaged;

        public void Free()
        {
            if (_unmanaged is not null)
            {
                _handle.Free();
            }
        }
    }

    public static class ElementIn
    {
        public static Rect ConvertToManaged(SDL_Rect rect) => new(rect.x, rect.y, rect.w, rect.h);

        public static SDL_Rect ConvertToUnmanaged(Rect rect) =>
            new()
            {
                x = rect.X,
                y = rect.Y,
                w = rect.Width,
                h = rect.Height,
            };
    }
}
