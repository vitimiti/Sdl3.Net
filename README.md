# Sdl3.Net

This is a fully managed, safe implementation and import of the SDL3 native library.
The library is not being reimplemented, but the API is safer and easier to use.

> **Disclaimer:**
>
> The XML documentation is created using AI: it may sound or look weird at points.
>
> Anybody is welcome to make this more human. If nobody does, I will when the project
> is complete, not before.
>
> This is a temporary measure to develop faster.

## SDL Error Reporting

All functions that may error in SDL3 will throw an `ExternalException` with a message
given by the library, followed by the message given by `SDL_GetError()`.

Ignoring these errors will terminate the program due to the exception system in dotnet,
forcing you to manage errors that shouldn't terminate your application.

## SDL Systems

Each SDL system is implemented in a safe, managed way to allow complete, safe use
of the library without the fuss of using C.

### The Properties System

The SDL3 properties system is managed by the class `Properties` in the `Sdl3.Net.PropertiesSystem`
namespace. This class is in charge of creating user properties and safely destroying them, as well
as getting the global properties.

For example, to create your own properties, you may:

```csharp
using namespace Sdl3.Net.PropertiesSystem;

using Properties props = Properties.Create();
props.SetProperty("my_property", "string_value");
props.SetProperty("my_second_property", 30);

Console.WriteLine($"My property string: {props.GetStringProperty("my_property", "NO VALUE")}");
Console.WriteLine($"My property number: {props.GetNumberProperty("my_second_property")}");
```

The getter methods have default values predetermined, and you only need to specify them when they are special,
like in the example above.

To get the global properties, simply call `var globalProps = Properties.GetGlobalProperties()`.

Other properties that are related to other systems will be discussed in said systems.

## The IO Stream System

To manage the SDL3 IO Streams, you may use the class `IoStream`, found in the `Sdl3.Net.StreamManagement`
namespace. This class inherits from dotnet's `Stream` and should be treated as such, even if it adds further
functionality related to SDL3.

To load a file and manage it, you may do:

```csharp
using Sdl3.Net.StreamManagement;

using IoStream stream = new("my_file.txt", IoStreamFileMode.OpenAndRead);
stream.Print("My text");

var streamProps = strem.GetProperties()
Console.WriteLine($"IO Stream file descriptor: {sreamProps.FileDescriptor}");
Console.Write($"IO Stream file pointer: 0x");
if (OperatingSystem.IsWindows())
{
    Console.WriteLine($"{streamProps.WindowsHandlePointer:X16}");
}
else
{
    Console.WriteLine($"{streamProps.StdioFilePointer:X16}");
}
```

Note that system properties will always have their own `GetProperties` function and have properties
to get, set, or get and set (depending on property), as shown in the example above.

Other methods such as `Seek` or `Write` or `Read` are implemented as per the `Stream` base class,
but using the SDL functions. The only method that cannot be implemented is `SetLength`, which will
throw a `NotSupportedException`.
