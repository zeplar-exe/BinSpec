namespace BinSpec;

public class ByteSpecifier
{
    public string Label { get; }
    public ulong ByteCount { get; }

    public ByteSpecifier(string label, ulong byteCount)
    {
        Label = label;
        ByteCount = byteCount;
    }
}