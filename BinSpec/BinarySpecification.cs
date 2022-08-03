namespace BinSpec;

public class BinarySpecification
{
    private List<ByteSpecifier> Specifiers { get; set; }

    public long SpecifiedLength => Specifiers.Sum(s => s.ByteCount);

    public BinarySpecification()
    {
        Specifiers = new List<ByteSpecifier>();
    }

    public ByteSpecifier AtLocation(long index)
    {
        var sum = 0L;

        foreach (var specifier in Specifiers)
        {
            if (index >= sum)
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

    public void CopyTo(BinarySpecification specification)
    {
        specification.Specifiers.AddRange(Specifiers);
    }
}