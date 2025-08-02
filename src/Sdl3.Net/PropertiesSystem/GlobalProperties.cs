using static Sdl3.Net.Imports.SDL3;

namespace Sdl3.Net.PropertiesSystem;

public class GlobalProperties : Properties
{
    internal GlobalProperties(SDL_PropertiesID propertiesId)
        : base(propertiesId) { }
}
