# Sdl3.Net

[![License: MIT](https://img.shields.io/github/license/vitimiti/Sdl3.Net)](https://opensource.org/licenses/MIT)
[![.NET](https://github.com/vitimiti/Sdl3.Net/actions/workflows/dotnet.yml/badge.svg)](https://github.com/vitimiti/Sdl3.Net/actions/workflows/dotnet.yml)
![Release](https://img.shields.io/github/v/release/vitimiti/Sdl3.Net)

![Stars](https://img.shields.io/github/stars/vitimiti/Sdl3.Net)
![Forks](https://img.shields.io/github/forks/vitimiti/Sdl3.Net)

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Visual Studio Code](https://img.shields.io/badge/Visual%20Studio%20Code-0078d7.svg?style=for-the-badge&logo=visual-studio-code&logoColor=white)

![Linux](https://img.shields.io/badge/Linux-FCC624?style=flat&logo=linux&logoColor=black)

This is a fully managed, safe implementation and import of the SDL3 native
library. The library is not being reimplemented, but the API is safer and easier
to use.

> <span style="color: red;">**Disclaimer**</span>
>
> The XML documentation is created using AI: it may sound or look weird at points.
>
> Anybody is welcome to make this more human. If nobody does, I will when the
> project is complete, not before.
>
> This is a temporary measure to develop faster.

Current version: `0.4.0-alpha`

> <span style="color: yellow;">**Note**</span>
>
> This is an "early access" project and will be heavily changing in a very short
> span of time.
>
> You should avoid using it until version 1.0.0 is reached.

## Table of Contents

- [Sdl3.Net](#sdl3net)
  - [Table of Contents](#table-of-contents)
  - [SDL Error Reporting](#sdl-error-reporting)
  - [SDL Systems](#sdl-systems)
    - [The Initialization System](#the-initialization-system)
    - [The Properties System](#the-properties-system)
    - [The IO Stream System](#the-io-stream-system)
    - [The Timer Subsystem](#the-timer-subsystem)

## SDL Error Reporting

All functions that may error in SDL3 will throw an `ExternalException` with a
message given by the library, followed by the message given by `SDL_GetError()`.

Ignoring these errors will terminate the program due to the exception system in
dotnet, forcing you to manage errors that shouldn't terminate your application.

## SDL Systems

Each SDL system is implemented in a safe, managed way to allow complete, safe
use of the library without the fuss of using C.

### The Initialization System

To initialize SDL and terminate it, you use the `App` class in the `Sdl3.Net`
namespace. This class provides the systems to set the SDL application metadata
and to initialize all subsystems.

For example:

```csharp
using namespace Sdl3.Net;
using namespace Sdl3.Net.AppSubsystems;
using namespace Sdl3.Net.Options;

using App app = new(options =>
{
    options.AppName = "My App";
    options.AppVersion = new Version(1, 2, 3);
    options.AppIdentifier = "org.app.my";
});

using var video = app.Video;
using var events = app.Events;
```

This library will automatically (in the example above), call `SDL_QuitSubSystem`
first for the `events`, then for the `video`, and then it will call `SDL_Quit`
for the `app`.

As with all SDL methods, initialization errors are transmitted to the user
through the `ExternalException`.

The app options are optional, and not providing any (`using App app = new()` or
`using App app = new(null)`) will not set any metadata and leave the SDL
defaults intact.

To set/get further metadata, you may use the static class `App.Metadata`. This
is not part of the options as it calls differente SDL functions and may have its
own erros, and therefore its own exceptions:

```csharp
using App app = new();
App.Metadata.SetName("My App");
Console.WriteLine($"App name: {App.Metadata.Name}");
```

### The Properties System

The SDL3 properties system is managed by the class `Properties` in the
`Sdl3.Net.PropertiesSystem` namespace. This class is in charge of creating user
properties and safely destroying them, as well as getting the global properties.

For example, to create your own properties, you may:

```csharp
using namespace Sdl3.Net.PropertiesSystem;

using Properties props = Properties.Create();
props.SetProperty("my_property", "string_value");
props.SetProperty("my_second_property", 30);

Console.WriteLine(
    $"My property string: {props.GetStringProperty("my_property", "NO VALUE")}"
);

Console.WriteLine($"My property number: {props.GetNumberProperty("my_second_property")}");
```

The getter methods have default values predetermined, and you only need to
specify them when they are special, like in the example above.

To get the global properties, simply call `var globalProps = Properties.GetGlobalProperties()`.

Other properties that are related to other systems will be discussed in said systems.

### The IO Stream System

To manage the SDL3 IO Streams, you may use the class `IoStream`, found in the
`Sdl3.Net.StreamManagement` namespace. This class inherits from dotnet's
`Stream` and should be treated as such, even if it adds further functionality
related to SDL3.

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

Note that system properties will always have their own `GetProperties` function
and have properties to get, set, or get and set (depending on property), as
shown in the example above.

Other methods such as `Seek` or `Write` or `Read` are implemented as per the
`Stream` base class, but using the SDL functions. The only method that cannot be
implemented is `SetLength`, which will throw a `NotSupportedException`.

### The Timer Subsystem

Becuase C# and dotnet already have timer systems, only the essentials have been added.

To get the time passed since the application initialization, you can do:

```csharp
using Sdl.Net;

using App app = new();
var timeSinceStartup = app.TimeSinceStartup;
```

And to get the performance counter and frequency, you can do:

```csharp
using Sdl.Net;

var performanceCounter = Performance.Counter;
var performanceFrequency = Performance.Frequency;
```
