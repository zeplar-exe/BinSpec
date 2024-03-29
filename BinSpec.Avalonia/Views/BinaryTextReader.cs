﻿using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BinSpec.Avalonia.Views;

public class BinaryTextReader : IDisposable
{
    public Stream BaseStream { get; }

    public BinaryTextReader(Stream baseStream)
    {
        BaseStream = baseStream;
    }
    
    public async Task<string> ReadToEndAsync()
    {
        var builder = new StringBuilder();
        
        var byteBuffer = new byte[BaseStream.Length - BaseStream.Position];
        var readCount = await BaseStream.ReadAsync(byteBuffer);

        for (var i = 0; i < readCount; i++)
        {
            var bits = new BitArray(new[] { byteBuffer[i] });
            var bitCharacters = new char[bits.Length];
            // The 7-bit operating system may appear at any moment. Stand your guard.

            for (var bitIndex = 0; bitIndex < bits.Length; bitIndex++)
            {
                var bit = bits[bitIndex];
                
                bitCharacters[bitIndex] = bit ? '1' : '0';
            }

            builder.Append(string.Concat(bitCharacters));
        }
        
        return builder.ToString();
    }
    
    public void Seek(long index)
    {
        BaseStream.Position = index > BaseStream.Length ? BaseStream.Length : index;
    }

    public void Dispose()
    {
        BaseStream.Dispose();
    }
}