using System;
using System.Collections.Generic;
using System.Text;

namespace Id3Parser;

public interface IMetadata
{
        // COMM
        string? Comments { get; }
        // PCNT
        int? PlayCounter { get; }
        // TALB
        string? Album { get; }
        // TBPM
        int? BPM { get; }
        // TDAT
        string? Date { get; }
        // TEXT
        string? Lyricist { get; }
        // TIME
        string? Time { get; }
        // TIT1
        string? ContentGroupDescription { get; }
        // TIT2
        string? Title { get; }
        // TIT3
        string? Subtitle { get; }
        // TYER
        DateTime? Year { get; }
        // TXXX
        string? Information { get; }
        // TPE2
        string? Band { get; }
        // TLEN
        TimeSpan? Length { get; }
        // TCON
        string? Genre { get; }
        // TPUB
        string? Publisher { get; }
        // TRACK
        int? TrackNumber { get; }
        // TRACK
        int? TrackTotal { get; }
    }
