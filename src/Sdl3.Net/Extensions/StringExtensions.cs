namespace Sdl3.Net.Extensions;

internal static class StringExtensions
{
    public static string ToStdIoString(this string str) => str.Replace("%", "%%");
}
