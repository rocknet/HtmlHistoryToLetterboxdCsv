using HtmlHistoryToLetterboxdCsv.Common;

namespace HtmlHistoryToLetterboxdCsv;

internal class App
{
    private readonly CommandLineOptions _options;
    private readonly HtmlParser _parser;

    public App(CommandLineOptions options, HtmlParser parser)
    {
        _options = options;
        _parser = parser;
    }

    public void Run()
    {
        if (_options.HistoryType == HistoryType.Plex)
            throw new NotImplementedException("Plex import is not done yet due to difficulty in getting full watch history export");

        Console.WriteLine($"Processing {_options.FileName} with {_options.HistoryType} processor.");

        _parser.Parse();
    }
}