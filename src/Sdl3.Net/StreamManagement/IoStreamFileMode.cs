namespace Sdl3.Net.StreamManagement;

public enum IoStreamFileMode
{
    OpenAndRead,
    CreateOrTruncateAndWrite,
    AppendOrCreateAndAppend,
    OpenAndReadWrite,
    CreateOrTruncateAndReadWrite,
    AppendOrCreateAndReadAppend,
    BinaryOpenAndRead,
    BinaryCreateOrTruncateAndWrite,
    BinaryAppendOrCreateAndAppend,
    BinaryOpenAndReadWrite,
    BinaryCreateOrTruncateAndReadWrite,
    BinaryAppendOrCreateAndReadAppend,
}
