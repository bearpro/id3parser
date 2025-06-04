using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Id3Parser.ID3v2.Frames;

internal class TextFrame : Frame
{
    /// <summary>
    /// Encoded text content of frame
    /// </summary>
    public override string Value { get; protected set; }

    public TextFrame(FrameHeader header, IEnumerable<byte> valueBytes) : base(header, valueBytes.ToArray())
    {
        Value = valueBytes.ElementAt(0) switch
        {
            0 => Encoding.ASCII.GetString(valueBytes.Skip(1).ToArray()),
            1 => Encoding.Unicode.GetString(valueBytes.Skip(1).ToArray()),
            _ => throw new NotImplementedException($"Encoding {valueBytes.ElementAt(0)} not implemented.")
        };
    }

    public override string ToString() => $"{FrameID}: {Value}";
}
