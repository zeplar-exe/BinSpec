using System;
using System.Collections;
using System.IO;

namespace BinSpec.Avalonia.Views;

public class SwapTextReader
{
    public Stream BaseStream { get; }

    public SwapTextReader(Stream baseStream)
    {
        BaseStream = baseStream;
    }
    
    public int Read(Span<string> buffer)
    {
        var byteBuffer = new byte[buffer.Length];
        var readCount = BaseStream.Read(byteBuffer);

        if (readCount < 1)
            return readCount;

        for (var i = 0; i < readCount; i++)
        {
            var bits = new BitArray(new[] { byteBuffer[i] });
            var bitCharacters = new char[bits.Length];
            // The 7-bit operating system may appear at any time. Stand your guard.

            for (var bitIndex = 0; bitIndex < bits.Length; bitIndex++)
            {
                var bit = bits[bitIndex];
                
                bitCharacters[bitIndex] = bit ? '1' : '0';
            }

            buffer[i] = string.Concat(bitCharacters);
        }
        
        return readCount;
    }
    
    public void Seek(long index)
    {
        BaseStream.Position = index > BaseStream.Length ? BaseStream.Length : index;
    }
}