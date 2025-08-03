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
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.PropertiesSystem;

/// <summary>
///  Represents a collection of properties that can be used to store various types of data.
/// </summary>
public class Properties : IDisposable
{
    /// <summary>
    ///  Delegate for cleanup callback when setting pointer properties.
    /// </summary>
    /// <param name="ptr">Pointer to the resource that needs cleanup.</param>
    public delegate void CleanupPropertyCallback(nint ptr);

    /// <summary>
    ///  Delegate for enumerating properties.
    /// </summary>
    /// <param name="properties">Properties object being enumerated.</param>
    /// <param name="name">Name of the property being enumerated.</param>
    public delegate void EnumeratePropertiesCallback(Properties properties, string name);

    private readonly bool _ownsProperties;

    private bool _isLocked;
    private bool _disposedValue;

    internal SDL_PropertiesID Id { get; }
    internal static GCHandle CleanupCallbackHandle { get; set; }

    internal Properties(SDL_PropertiesID propertiesId, bool ownsProperties = false) =>
        (Id, _ownsProperties) = (propertiesId, ownsProperties);

    /// <summary>
    ///  Unlocks the properties if they are locked, disposes of the cleanup callback handle,
    ///  and destroys the properties if they are owned.
    /// </summary>
    /// <param name="disposing">Indicates whether the method is called from Dispose or finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        if (disposing)
        {
            // Nothing to dispose of in managed resources
        }

        if (_isLocked)
        {
            SDL_LockProperties(Id);
            _isLocked = false;
        }

        if (CleanupCallbackHandle.IsAllocated)
        {
            CleanupCallbackHandle.Free();
        }

        if (_ownsProperties)
        {
            SDL_DestroyProperties(Id);
        }

        _disposedValue = true;
    }

    /// <summary>
    ///  Releases the resources used by the Properties instance.
    /// </summary>
    ~Properties()
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

    /// <summary>
    ///  Creates a new instance of Properties.
    /// </summary>
    /// <returns>A new Properties instance.</returns>
    /// <exception cref="ExternalException">Thrown if the properties could not be created.</exception>
    public static Properties Create()
    {
        SDL_PropertiesID propertiesId = SDL_CreateProperties();
        return propertiesId.Value == 0
            ? throw new ExternalException($"Failed to create properties: {SDL_GetError()}")
            : new Properties(propertiesId, ownsProperties: true);
    }

    /// <summary>
    ///  Gets the global properties.
    /// </summary>
    /// <returns>A GlobalProperties instance representing the global properties.</returns>
    /// <exception cref="ExternalException">Thrown if the global properties could not be retrieved.</exception>
    public static GlobalProperties GetGlobal()
    {
        SDL_PropertiesID propertiesId = SDL_GetGlobalProperties();
        return propertiesId.Value == 0
            ? throw new ExternalException($"Failed to get global properties: {SDL_GetError()}")
            : new GlobalProperties(propertiesId);
    }

    /// <summary>
    ///  Checks if a property with the specified name exists.
    /// </summary>
    /// <param name="name">The name of the property to check.</param>
    /// <returns>True if the property exists, otherwise false.</returns>
    public bool HasProperty(string name) => SDL_HasProperty(Id, name);

    /// <summary>
    ///  Gets the type of a property by its name.
    /// </summary>
    /// <param name="name">The name of the property to get the type for.</param>
    /// <returns>The PropertyType of the specified property.</returns>
    public PropertyType GetPropertyType(string name) => SDL_GetPropertyType(Id, name);

    /// <summary>
    ///  Gets a pointer property by its name.
    /// </summary>
    /// <param name="name">The name of the property to get.</param>
    /// <param name="defaultValue">The default value to return if the property is not found.</param>
    /// <returns>The pointer value of the specified property.</returns>
    /// <remarks>This will safely attempt to lock the properties before getting the value.</remarks>
    public nint GetPointerProperty(string name, nint defaultValue = 0)
    {
        var locked = SDL_LockProperties(Id);
        nint value = SDL_GetPointerProperty(Id, name, defaultValue);
        if (locked)
        {
            SDL_UnlockProperties(Id);
        }

        return value;
    }

    /// <summary>
    ///  Gets a string property by its name.
    /// </summary>
    /// <param name="name">The name of the property to get.</param>
    /// <param name="defaultValue">The default value to return if the property is not found.</param>
    /// <returns>The string value of the specified property.</returns>
    /// <remarks>This will safely attempt to lock the properties before getting the value.</remarks>
    public string? GetStringProperty(string name, string? defaultValue = null)
    {
        var locked = SDL_LockProperties(Id);
        string? value = SDL_GetStringProperty(Id, name, defaultValue);
        if (locked)
        {
            SDL_UnlockProperties(Id);
        }

        return value;
    }

    /// <summary>
    ///  Gets a number property by its name.
    /// </summary>
    /// <param name="name">The name of the property to get.</param>
    /// <param name="defaultValue">The default value to return if the property is not found.</param>
    /// <returns>The number value of the specified property.</returns>
    public long GetNumberProperty(string name, long defaultValue = 0) =>
        SDL_GetNumberProperty(Id, name, defaultValue);

    /// <summary>
    ///  Gets a float property by its name.
    /// </summary>
    /// <param name="name">The name of the property to get.</param>
    /// <param name="defaultValue">The default value to return if the property is not found.</param>
    /// <returns>The float value of the specified property.</returns>
    public float GetFloatProperty(string name, float defaultValue = .0F) =>
        SDL_GetFloatProperty(Id, name, defaultValue);

    /// <summary>
    ///  Gets a boolean property by its name.
    /// </summary>
    /// <param name="name">The name of the property to get.</param>
    /// <param name="defaultValue">The default value to return if the property is not found.</param>
    /// <returns>The boolean value of the specified property.</returns>
    public bool GetBoolProperty(string name, bool defaultValue = false) =>
        SDL_GetBoolProperty(Id, name, defaultValue);

    /// <summary>
    ///  Sets a pointer property with a cleanup callback.
    /// </summary>
    /// <param name="name">The name of the property to set.</param>
    /// <param name="value">The pointer value to set.</param>
    /// <param name="cleanup">The cleanup callback to invoke when the property is no longer needed.</param>
    /// <remarks>This will safely attempt to lock the properties before setting the value.</remarks>
    /// <exception cref="ExternalException">Thrown if the property could not be set.</exception>
    public void SetProperty(string name, nint value, CleanupPropertyCallback cleanup)
    {
        var locked = SDL_LockProperties(Id);
        if (!SDL_SetPointerPropertyWithCleanup(Id, name, value, cleanup))
        {
            throw new ExternalException(
                $"Failed to set pointer property '{name}' for properties '0x{Id:X8}' to '0x{value:X8}': {SDL_GetError()}"
            );
        }

        if (locked)
        {
            SDL_UnlockProperties(Id);
        }
    }

    /// <summary>
    ///  Sets a pointer property without a cleanup callback.
    /// </summary>
    /// <param name="name">The name of the property to set.</param>
    /// <param name="value">The pointer value to set.</param>
    /// <remarks>This will safely attempt to lock the properties before setting the value.</remarks>
    /// <exception cref="ExternalException">Thrown if the property could not be set.</exception>
    public void SetProperty(string name, nint value)
    {
        var locked = SDL_LockProperties(Id);
        if (!SDL_SetPointerProperty(Id, name, value))
        {
            throw new ExternalException(
                $"Failed to set pointer property '{name}' for properties '0x{Id:X8}' to '0x{value:X8}': {SDL_GetError()}"
            );
        }

        if (locked)
        {
            SDL_UnlockProperties(Id);
        }
    }

    /// <summary>
    ///  Sets a string property.
    /// </summary>
    /// <param name="name">The name of the property to set.</param>
    /// <param name="value">The string value to set.</param>
    /// <remarks> This will safely attempt to lock the properties before setting the value.</remarks>
    /// <exception cref="ExternalException">Thrown if the property could not be set.</exception>
    public void SetProperty(string name, string? value)
    {
        var locked = SDL_LockProperties(Id);
        if (!SDL_SetStringProperty(Id, name, value))
        {
            throw new ExternalException(
                $"Failed to set string property '{name}' for properties '0x{Id:X8}' to '{value}': {SDL_GetError()}"
            );
        }

        if (locked)
        {
            SDL_UnlockProperties(Id);
        }
    }

    /// <summary>
    ///  Sets a number property.
    /// </summary>
    /// <param name="name">The name of the property to set.</param>
    /// <param name="value">The number value to set.</param>
    /// <exception cref="ExternalException">Thrown if the property could not be set.</exception>
    public void SetProperty(string name, long value)
    {
        if (!SDL_SetNumberProperty(Id, name, value))
        {
            throw new ExternalException(
                $"Failed to set number property '{name}' for properties '0x{Id:X8}' to '{value}': {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    ///  Sets a float property.
    /// </summary>
    /// <param name="name">The name of the property to set.</param>
    /// <param name="value">The float value to set.</param>
    /// <exception cref="ExternalException">Thrown if the property could not be set.</exception>
    public void SetProperty(string name, float value)
    {
        if (!SDL_SetNumberProperty(Id, name, (long)value))
        {
            throw new ExternalException(
                $"Failed to set number property '{name}' for properties '0x{Id:X8}' to '{value:F2}': {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    ///  Sets a boolean property.
    /// </summary>
    /// <param name="name">The name of the property to set.</param>
    /// <param name="value">The boolean value to set.</param>
    /// <exception cref="ExternalException">Thrown if the property could not be set.</exception>
    public void SetProperty(string name, bool value)
    {
        if (!SDL_SetBoolProperty(Id, name, value))
        {
            throw new ExternalException(
                $"Failed to set boolean property '{name}' for properties '0x{Id:X8}' to '{value}': {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    ///  Enumerates all properties in the collection.
    /// </summary>
    /// <param name="callback">The callback to invoke for each property.</param>
    /// <exception cref="ExternalException">Thrown if the enumeration fails.</exception>
    public void EnumerateProperties(EnumeratePropertiesCallback callback)
    {
        if (!SDL_EnumerateProperties(Id, callback))
        {
            throw new ExternalException(
                $"Failed to enumerate properties '0x{Id:X8}': {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    ///  Clears a property by its name.
    /// </summary>
    /// <param name="name">The name of the property to clear.</param>
    /// <exception cref="ExternalException">Thrown if the property could not be cleared.</exception>
    public void ClearProperty(string name)
    {
        if (!SDL_ClearProperty(Id, name))
        {
            throw new ExternalException(
                $"Failed to clear property '{name}' for properties '0x{Id:X8}': {SDL_GetError()}"
            );
        }
    }

    /// <summary>
    ///  Locks the properties for exclusive access.
    /// </summary>
    /// <exception cref="ExternalException">Thrown if the properties could not be locked.</exception>
    public void Lock()
    {
        if (!SDL_LockProperties(Id))
        {
            throw new ExternalException($"Failed to lock properties: {SDL_GetError()}");
        }

        _isLocked = true;
    }

    /// <summary>
    ///  Unlocks the properties after exclusive access.
    /// </summary>
    public void Unlock() => SDL_UnlockProperties(Id);

    /// <summary>
    ///  Copies the properties to another Properties instance.
    /// </summary>
    /// <param name="destination">The destination Properties instance.</param>
    /// <exception cref="ExternalException">Thrown if the properties could not be copied.</exception>
    public void CopyTo(Properties destination)
    {
        if (!SDL_CopyProperties(Id, destination.Id))
        {
            throw new ExternalException(
                $"Failed to copy properties '0x{Id:X8}' to properties '0x{destination.Id:X8}': {SDL_GetError()}"
            );
        }
    }
}
