namespace BinSpec;

public class ByteSpecifier
{
    public string DebugTypeName { get; }
    public long ByteCount { get; }
    
    public ByteSpecifier(long byteCount) : this("UnknownType", byteCount)
    {
        
    }
    
    public ByteSpecifier(string debugTypeName, long byteCount)
    {
        if (byteCount < 1)
            throw new ArgumentException("Byte count must be larger than 0.", nameof(byteCount));
        
        DebugTypeName = debugTypeName;
        ByteCount = byteCount;
    }
}