using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace Id3Parser;

public class V23Parser
{
        public static IMetadata Parse(Stream stream)
        {
            byte[] buffer;
            if (!stream.CanRead) throw new IOException("Can't read stream");
            int startindex = ID3v2Parser.FindID3Index(stream);
            buffer = new byte[10];
            stream.Read(buffer, 0, 10);
            var header = new ID3v2.TagHeader(buffer);
            buffer = new byte[header.Length];
            stream.Read(buffer, startindex, header.Length);
            IEnumerable<byte> eBuffer = buffer;
            var frames = new LinkedList<ID3v2.Frame>();
            while (eBuffer.Count() > 10)
            {
                ID3v2.FrameHeader frameHeader;
                var frameHeaderbytes = eBuffer.Take(10);
                try
                {
                    frameHeader = new ID3v2.FrameHeader(frameHeaderbytes.ToArray());
                }
                catch (FormatException)
                {
                    break;
                }
                ID3v2.Frame frame;
                if (frameHeader.FrameID[0] == 'T')
                {
                    frame = new ID3v2.Frames.TextFrame(frameHeader,
                                                valueBytes: eBuffer.Skip(10)
                                                             .Take(frameHeader.Length)
                                                             .ToArray());
                }
                else
                {
                    frame = new ID3v2.Frames.Raw(frameHeader,
                                                valueBytes: eBuffer.Skip(10)
                                                             .Take(frameHeader.Length)
                                                             .ToArray());
                }
                frames.AddLast(frame);
                eBuffer = eBuffer.Skip(frame.Length);
            }
            var tag = new ID3v2.Tag(header, frames);
            return tag;
            throw new NotImplementedException();
        }
        public static IMetadata Parse(string path)
        {
            using (FileStream f = File.OpenRead(path))
            {
                IMetadata m = Parse(f);
                return m;
            }
        }
    }
    internal class ID3v2Parser
    {
        /// <summary>
        /// Returns ID3 start index
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static int FindID3Index(Stream stream)
        {
            byte[] buffer = new byte[3];
            for (int i = 0; i < stream.Length-3; i++)
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
