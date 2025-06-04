using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Id3Parser;

public class V23Parser
{
    public static IMetadata Parse(Stream stream)
    {
        if (!stream.CanRead) throw new IOException("Can't read stream");
        var startIndex = ID3v2Parser.FindID3Index(stream);

        var buffer = new byte[10];
        stream.Read(buffer, 0, 10);
        var header = new ID3v2.TagHeader(buffer);

        buffer = new byte[header.Length];
        stream.Read(buffer, startIndex, header.Length);
        IEnumerable<byte> eBuffer = buffer;
        var frames = new LinkedList<ID3v2.Frame>();
        while (eBuffer.Count() > 10)
        {
            var frameHeaderBytes = eBuffer.Take(10).ToArray();
            ID3v2.FrameHeader frameHeader;
            try
            {
                frameHeader = new ID3v2.FrameHeader(frameHeaderBytes);
            }
            catch (FormatException)
            {
                break;
            }

            ID3v2.Frame frame = frameHeader.FrameID[0] == 'T'
                ? new ID3v2.Frames.TextFrame(frameHeader, eBuffer.Skip(10).Take(frameHeader.Length).ToArray())
                : new ID3v2.Frames.Raw(frameHeader, eBuffer.Skip(10).Take(frameHeader.Length).ToArray());

            frames.AddLast(frame);
            eBuffer = eBuffer.Skip(frame.Length);
        }
        var tag = new ID3v2.Tag(header, frames);
        return tag;
    }

    public static IMetadata Parse(string path)
    {
        using FileStream f = File.OpenRead(path);
        return Parse(f);
    }
}

internal static class ID3v2Parser
{
    /// <summary>
    /// Returns ID3 start index
    /// </summary>
    internal static int FindID3Index(Stream stream)
    {
        var buffer = new byte[3];
        for (int i = 0; i < stream.Length - 3; i++)
        {
            stream.Read(buffer, 0, 3);
            if (ID3v2.TagHeader.MarkerBytes.SequenceEqual(buffer))
            {
                stream.Position = i;
                return i;
            }
            stream.Position = i;
        }
        throw new Exception("No ID3 in file");
    }
}
