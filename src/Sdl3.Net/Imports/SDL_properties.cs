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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Sdl3.Net.CustomMarshallers;
using Sdl3.Net.PropertiesSystem;

namespace Sdl3.Net.Imports;

internal static partial class SDL3
{
    public record struct SDL_PropertiesID(uint Value);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_GetGlobalProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_PropertiesID SDL_GetGlobalProperties();

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CreateProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial SDL_PropertiesID SDL_CreateProperties();

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_CopyProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_CopyProperties(SDL_PropertiesID src, SDL_PropertiesID dst);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_LockProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static partial bool SDL_LockProperties(SDL_PropertiesID propertiesId);

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_UnlockProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_UnlockProperties(SDL_PropertiesID propertiesId);

    private static readonly unsafe delegate* unmanaged[Cdecl]<
        nint,
        nint,
        void> SDL_CleanupPropertyCallbackPtr = &SDL_CleanupPropertyCallbackImpl;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void SDL_CleanupPropertyCallbackImpl(nint userdata, nint value)
    {
        if (userdata == nint.Zero)
        {
            return;
        }

        if (GCHandle.FromIntPtr(userdata).Target is not Properties.CleanupPropertyCallback callback)
        {
            return;
        }

        callback(value);
    }

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetPointerPropertyWithCleanup),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    private static unsafe partial bool SDL_SetPointerPropertyWithCleanupInternal(
        SDL_PropertiesID props,
        string name,
        nint value,
        delegate* unmanaged[Cdecl]<nint, nint, void> cleanup,
        nint userdata
    );

    public static unsafe bool SDL_SetPointerPropertyWithCleanup(
        SDL_PropertiesID props,
        string name,
        nint value,
        Properties.CleanupPropertyCallback cleanup
    )
    {
        Properties.CleanupCallbackHandle = GCHandle.Alloc(cleanup);
        return SDL_SetPointerPropertyWithCleanupInternal(
            props,
            name,
            value,
            SDL_CleanupPropertyCallbackPtr,
            GCHandle.ToIntPtr(Properties.CleanupCallbackHandle)
        );
    }

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetPointerProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetPointerProperty(
        SDL_PropertiesID props,
        string name,
        nint value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetStringProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetStringProperty(
        SDL_PropertiesID props,
        string name,
        string? value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetNumberProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetNumberProperty(
        SDL_PropertiesID props,
        string name,
        long value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetFloatProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetFloatProperty(
        SDL_PropertiesID props,
        string name,
        float value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_SetBoolProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_SetBoolProperty(
        SDL_PropertiesID props,
        string name,
        [MarshalAs(UnmanagedType.U1)] bool value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_HasProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_HasProperty(SDL_PropertiesID props, string name);

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetPropertyType),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial PropertyType SDL_GetPropertyType(
        SDL_PropertiesID props,
        string name
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetPointerProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial nint SDL_GetPointerProperty(
        SDL_PropertiesID props,
        string name,
        nint default_value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetStringProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalUsing(typeof(SdlOwnedUtf8StringMarshaller))]
    public static partial string? SDL_GetStringProperty(
        SDL_PropertiesID props,
        string name,
        string? default_value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetNumberProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial long SDL_GetNumberProperty(
        SDL_PropertiesID props,
        string name,
        long default_value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetFloatProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial float SDL_GetFloatProperty(
        SDL_PropertiesID props,
        string name,
        float default_value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_GetBoolProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_GetBoolProperty(
        SDL_PropertiesID props,
        string name,
        [MarshalAs(UnmanagedType.U1)] bool default_value
    );

    [LibraryImport(
        nameof(SDL3),
        EntryPoint = nameof(SDL_ClearProperty),
        StringMarshalling = StringMarshalling.Utf8
    )]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    public static unsafe partial bool SDL_ClearProperty(SDL_PropertiesID props, string name);

    private static readonly unsafe delegate* unmanaged[Cdecl]<
        nint,
        SDL_PropertiesID,
        byte*,
        void> SDL_EnumeratePropertiesCallbackPtr = &SDL_EnumeratePropertiesCallbackImpl;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static unsafe void SDL_EnumeratePropertiesCallbackImpl(
        nint userdata,
        SDL_PropertiesID propertiesId,
        byte* name
    )
    {
        if (userdata == nint.Zero || name is null || propertiesId.Value == 0)
        {
            return;
        }

        if (
            GCHandle.FromIntPtr(userdata).Target
            is not Properties.EnumeratePropertiesCallback callback
        )
        {
            return;
        }

        // We have already checked for null and zero values, so we can safely convert the name.
        callback(new Properties(propertiesId), Utf8StringMarshaller.ConvertToManaged(name)!);
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_EnumerateProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.U1)]
    private static unsafe partial bool SDL_EnumeratePropertiesInternal(
        SDL_PropertiesID propertiesId,
        delegate* unmanaged[Cdecl]<nint, SDL_PropertiesID, byte*, void> callback,
        nint userdata
    );

    public static unsafe bool SDL_EnumerateProperties(
        SDL_PropertiesID propertiesId,
        Properties.EnumeratePropertiesCallback callback
    )
    {
        Properties.CleanupCallbackHandle = GCHandle.Alloc(callback);

        try
        {
            return SDL_EnumeratePropertiesInternal(
                propertiesId,
                SDL_EnumeratePropertiesCallbackPtr,
                GCHandle.ToIntPtr(Properties.CleanupCallbackHandle)
            );
        }
        finally
        {
            if (Properties.CleanupCallbackHandle.IsAllocated)
            {
                Properties.CleanupCallbackHandle.Free();
            }
        }
    }

    [LibraryImport(nameof(SDL3), EntryPoint = nameof(SDL_DestroyProperties))]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void SDL_DestroyProperties(SDL_PropertiesID propertiesId);
}
