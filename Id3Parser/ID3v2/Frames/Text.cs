using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Id3Parser.ID3v2.Frames;

class TextFrame : Frame
{
        /// <summary>
        /// Encoded text content of frame
        /// </summary>
        public override string Value { get; protected set; }
        public TextFrame(FrameHeader header, IEnumerable<byte> valueBytes) : base(header, valueBytes.ToArray())
        {
            if (valueBytes.ElementAt(0) == 0)
                Value = Encoding.ASCII.GetString(valueBytes.Skip(1).ToArray());
            else if (valueBytes.ElementAt(0) == 1)
                Value = Encoding.Unicode.GetString(valueBytes.Skip(1).ToArray());
            else
                throw new NotImplementedException($"Encoding {valueBytes.ElementAt(0)} not implemented.");
        }
        public override string ToString()
        {
            return $"{FrameID}: {Value}";
        }
    }
