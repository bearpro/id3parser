using System;
using System.Collections.Generic;
using System.Linq;

namespace Id3Parser.ID3v2;

public class Tag : IMetadata
{
    public TagHeader Header { get; }
    public LinkedList<Frame> Frames { get; }

    public string? Comments { get; }
    public int? PlayCounter { get; }
    public string? Album { get; }
    public int? BPM { get; }
    public string? Date { get; }
    public string? Lyricist { get; }
    public string? Time { get; }
    public string? ContentGroupDescription { get; }
    public string? Title { get; }
    public string? Subtitle { get; }
    public DateTime? Year { get; }
    public string? Information { get; }
    public string? Band { get; }
    public TimeSpan? Length { get; }
    public string? Genre { get; }
    public string? Publisher { get; }
    public int? TrackNumber { get; }
    public int? TrackTotal { get; }

    public Tag(TagHeader header, IEnumerable<Frame> frames)
    {
        Header = header;
        Frames = new LinkedList<Frame>(frames);

        foreach (var frame in frames)
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
                    if (int.TryParse(frame.Value, out var year))
                        Year = new DateTime(year, 1, 1);
                    break;
                case "TIT3":
                    Subtitle = frame.Value;
                    break;
                case "TBPM":
                    BPM = int.Parse(frame.Value);
                    break;
                case "TLEN":
                    if (int.TryParse(frame.Value, out var length))
                        Length = TimeSpan.FromMilliseconds(length);
                    break;
                case "TCON":
                    Genre = frame.Value;
                    break;
                case "TPUB":
                    Publisher = frame.Value;
                    break;
                case "TRCK":
                    if (frame.Value.Contains('/'))
                    {
                        var parts = frame.Value.Split('/');
                        if (int.TryParse(parts[0], out var trackNumber))
                            TrackNumber = trackNumber;
                        if (int.TryParse(parts[1], out var trackTotal))
                            TrackTotal = trackTotal;
                    }
                    else if (int.TryParse(frame.Value, out var track))
                    {
                        TrackNumber = track;
                    }
                    break;
            }
        }
    }
}
