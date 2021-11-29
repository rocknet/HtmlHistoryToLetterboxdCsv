using System.Globalization;

namespace HtmlHistoryToLetterboxdCsv.Common;

internal class HtmlHistoryToLetterboxdConfig
{
    public string ExportFolder { get; set; } = "Export/";
    public Dictionary<string, ProcessorConfig> Processors { get; set; } = new();
}

internal class ProcessorConfig
{
    public string? Tags { get; set; }
    public int WatchedDateAddDays { get; set; }
    public string WatchedDateFormat { get; set; } = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
    public Dictionary<string, string> XPaths { get; set; } = new();
}