using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Id3Parser.ID3v2;

public class Tag : IMetadata
{
        public TagHeader Header { get; protected set; }
        public LinkedList<Frame> Frames { get; protected set; }

        public string? Comments { get; protected set; }

        public int? PlayCounter { get; protected set; }

        public string? Album { get; protected set; }

        public int? BPM { get; protected set; }

        public string? Date { get; protected set; }

        public string? Lyricist { get; protected set; }

        public string? Time { get; protected set; }

        public string? ContentGroupDescription { get; protected set; }

        public string? Title { get; protected set; }

        public string? Subtitle { get; protected set; }

        public DateTime? Year { get; protected set; }

        public string? Information { get; protected set; }

        public string? Band { get; protected set; }

        public TimeSpan? Length { get; protected set; }

        public string? Genre { get; protected set; }

        public string? Publisher { get; protected set; }

        public int? TrackNumber { get; protected set; }

        public int? TrackTotal { get; protected set; }

        public Tag(TagHeader header, IEnumerable<Frame> frames)
        {
            Header = header;
            Frames = new LinkedList<Frame>(frames);
            foreach (Frame frame in frames)
            {
                switch (frame.FrameID)
                {
                    case "TIT2":
                        Title = frame.Value;
                        break;
                    case "TALB":
                        Album = frame.Value;
                        break;
                    case "TPE1":
                        Band = frame.Value;
                        break;
                    case "TDAT":
                        Date = frame.Value;
                        break;
                    case "TYER":
                        int year;
                        if (int.TryParse(frame.Value, out year))
                            Year = new DateTime(year: year, month: 1, day: 1);
                        break;
                    case "TIT3":
                        Subtitle = frame.Value;
                        break;
                    case "TBPM":
                        BPM = int.Parse(frame.Value);
                        break;
                    case "TLEN":
                        int length;
                        if (int.TryParse(frame.Value, out length))
                            Length = TimeSpan.FromMilliseconds(length);
                        break;
                    case "TCON":
                        Genre = frame.Value;
                        break;
                    case "TPUB":
                        Publisher = frame.Value;
                        break;
                    case "TRCK":
                        if (frame.Value.Contains("/"))
                        {
                            var trackNumberString = frame.Value.Split('/')[0];
                            var trackTotalString = frame.Value.Split('/')[1];
                            if (int.TryParse(trackNumberString, out int trackNumber))
                                TrackNumber = trackNumber;
                            if (int.TryParse(trackTotalString, out int trackTotal))
                                TrackTotal = trackTotal;
                            break;
                        }
                        else
                        {
                            if (int.TryParse(frame.Value, out int trackNumber))
                                TrackNumber = trackNumber;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
