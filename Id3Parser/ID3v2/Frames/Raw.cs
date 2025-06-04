using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Id3Parser.ID3v2.Frames;

public class Raw : Frame
{
    public Raw(FrameHeader header, IEnumerable<byte> valueBytes) : base(header, valueBytes.ToArray())
    {

    }
}
