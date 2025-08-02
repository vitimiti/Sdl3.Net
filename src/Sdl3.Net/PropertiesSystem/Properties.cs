using System.Runtime.InteropServices;
using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.PropertiesSystem;

public class Properties : IDisposable
{
    public delegate void CleanupPropertyCallback(nint ptr);

    public delegate void EnumeratePropertiesCallback(Properties properties, string name);

    private SDL_PropertiesID _propertiesId;
    private bool _isLocked;
    private bool _disposedValue;

    internal static GCHandle CleanupCallbackHandle { get; set; }

    internal Properties(SDL_PropertiesID propertiesId) => _propertiesId = propertiesId;

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
            SDL_LockProperties(_propertiesId);
            _isLocked = false;
        }

        if (CleanupCallbackHandle.IsAllocated)
        {
            CleanupCallbackHandle.Free();
        }

        SDL_DestroyProperties(_propertiesId);
        _disposedValue = true;
    }

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

    public static Properties Create()
    {
        SDL_PropertiesID propertiesId = SDL_CreateProperties();
        return propertiesId.Value == 0
            ? throw new ExternalException($"Failed to create properties: {SDL_GetError()}")
            : new Properties(propertiesId);
    }

    public static GlobalProperties GetGlobal()
    {
        SDL_PropertiesID propertiesId = SDL_GetGlobalProperties();
        return propertiesId.Value == 0
            ? throw new ExternalException($"Failed to get global properties: {SDL_GetError()}")
            : new GlobalProperties(propertiesId);
    }

    public bool HasProperty(string name) => SDL_HasProperty(_propertiesId, name);

    public PropertyType GetPropertyType(string name) => SDL_GetPropertyType(_propertiesId, name);

    public nint GetPointerProperty(string name, nint defaultValue = 0)
    {
        var locked = SDL_LockProperties(_propertiesId);
        nint value = SDL_GetPointerProperty(_propertiesId, name, defaultValue);
        if (locked)
        {
            SDL_UnlockProperties(_propertiesId);
        }

        return value;
    }

    public string? GetStringProperty(string name, string? defaultValue = null)
    {
        var locked = SDL_LockProperties(_propertiesId);
        string? value = SDL_GetStringProperty(_propertiesId, name, defaultValue);
        if (locked)
        {
            SDL_UnlockProperties(_propertiesId);
        }

        return value;
    }

    public long GetNumberProperty(string name, long defaultValue = 0) =>
        SDL_GetNumberProperty(_propertiesId, name, defaultValue);

    public float GetFloatProperty(string name, float defaultValue = .0F) =>
        SDL_GetFloatProperty(_propertiesId, name, defaultValue);

    public bool GetBoolProperty(string name, bool defaultValue = false) =>
        SDL_GetBoolProperty(_propertiesId, name, defaultValue);

    public void SetProperty(string name, nint value, CleanupPropertyCallback cleanup)
    {
        var locked = SDL_LockProperties(_propertiesId);
        if (!SDL_SetPointerPropertyWithCleanup(_propertiesId, name, value, cleanup))
        {
            throw new ExternalException(
                $"Failed to set pointer property '{name}' for properties '0x{_propertiesId:X8}' to '0x{value:X8}': {SDL_GetError()}"
            );
        }

        if (locked)
        {
            SDL_UnlockProperties(_propertiesId);
        }
    }

    public void SetProperty(string name, nint value)
    {
        var locked = SDL_LockProperties(_propertiesId);
        if (!SDL_SetPointerProperty(_propertiesId, name, value))
        {
            throw new ExternalException(
                $"Failed to set pointer property '{name}' for properties '0x{_propertiesId:X8}' to '0x{value:X8}': {SDL_GetError()}"
            );
        }

        if (locked)
        {
            SDL_UnlockProperties(_propertiesId);
        }
    }

    public void SetProperty(string name, string? value)
    {
        var locked = SDL_LockProperties(_propertiesId);
        if (!SDL_SetStringProperty(_propertiesId, name, value))
        {
            throw new ExternalException(
                $"Failed to set string property '{name}' for properties '0x{_propertiesId:X8}' to '{value}': {SDL_GetError()}"
            );
        }

        if (locked)
        {
            SDL_UnlockProperties(_propertiesId);
        }
    }

    public void SetProperty(string name, long value)
    {
        if (!SDL_SetNumberProperty(_propertiesId, name, value))
        {
            throw new ExternalException(
                $"Failed to set number property '{name}' for properties '0x{_propertiesId:X8}' to '{value}': {SDL_GetError()}"
            );
        }
    }

    public void SetProperty(string name, float value)
    {
        if (!SDL_SetNumberProperty(_propertiesId, name, (long)value))
        {
            throw new ExternalException(
                $"Failed to set number property '{name}' for properties '0x{_propertiesId:X8}' to '{value:F2}': {SDL_GetError()}"
            );
        }
    }

    public void SetProperty(string name, bool value)
    {
        if (!SDL_SetBoolProperty(_propertiesId, name, value))
        {
            throw new ExternalException(
                $"Failed to set boolean property '{name}' for properties '0x{_propertiesId:X8}' to '{value}': {SDL_GetError()}"
            );
        }
    }

    public void EnumerateProperties(EnumeratePropertiesCallback callback)
    {
        if (!SDL_EnumerateProperties(_propertiesId, callback))
        {
            throw new ExternalException(
                $"Failed to enumerate properties '0x{_propertiesId:X8}': {SDL_GetError()}"
            );
        }
    }

    public void ClearProperty(string name)
    {
        if (!SDL_ClearProperty(_propertiesId, name))
        {
            throw new ExternalException(
                $"Failed to clear property '{name}' for properties '0x{_propertiesId:X8}': {SDL_GetError()}"
            );
        }
    }

    public void Lock()
    {
        if (_isLocked)
        {
            throw new InvalidOperationException(
                $"Properties '0x{_propertiesId:X8}' are already locked."
            );
        }

        if (!SDL_LockProperties(_propertiesId))
        {
            throw new ExternalException($"Failed to lock properties: {SDL_GetError()}");
        }

        _isLocked = true;
    }

    public void Unlock()
    {
        if (!_isLocked)
        {
            throw new InvalidOperationException(
                $"Properties '0x{_propertiesId:X8}' are not locked."
            );
        }

        SDL_UnlockProperties(_propertiesId);
        _isLocked = false;
    }

    public void CopyTo(Properties destination)
    {
        if (!SDL_CopyProperties(_propertiesId, destination._propertiesId))
        {
            throw new ExternalException(
                $"Failed to copy properties '0x{_propertiesId:X8}' to properties '0x{destination._propertiesId:X8}': {SDL_GetError()}"
            );
        }
    }
}
