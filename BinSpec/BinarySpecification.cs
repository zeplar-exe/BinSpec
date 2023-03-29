namespace BinSpec;

public class BinarySpecification
{
    private List<ByteSpecifier> Specifiers { get; }

    // https://stackoverflow.com/a/35613036/16324801
    public ulong SpecifiedLength => Specifiers
        .Select(s => s.ByteCount)
        .Aggregate((a, c) => a + c);
    
    public int BitSize { get; }

    public BinarySpecification(int bitSize)
    {
        BitSize = bitSize;
        Specifiers = new List<ByteSpecifier>();
    }

    public ByteSpecifier AtLocation(ulong index)
    {
        var sum = 0UL;

        foreach (var specifier in Specifiers)
        {
            if (index < sum)
            {
                return specifier;
            }
            
            sum += specifier.ByteCount - 1;
        }
        
        throw new IndexOutOfRangeException(
            $"'{nameof(index)}' ({index}) is outside the bounds of this specification; '{SpecifiedLength}' bytes.");
    }

    public void AddSpecifier(ByteSpecifier specifier)
    {
        Specifiers.Add(specifier);
    }
}