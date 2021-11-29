using System.Diagnostics.CodeAnalysis;
using CommandLine;

namespace HtmlHistoryToLetterboxdCsv.Common;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
internal enum HistoryType
{
    Missing = 0,
    NetflixDvd,
    Amazon,
    Plex
}

internal class CommandLineOptions
{
    [Option(Required = true)]
    public string? FileName { get; set; }

    [Option(Required = true)]
    public HistoryType HistoryType { get; set; }
}