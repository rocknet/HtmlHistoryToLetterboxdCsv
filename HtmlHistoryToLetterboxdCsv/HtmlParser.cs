using System.Globalization;
using CsvHelper;
using HtmlAgilityPack;
using HtmlHistoryToLetterboxdCsv.Common;
using HtmlHistoryToLetterboxdCsv.Dtos;
using Microsoft.Extensions.Options;

namespace HtmlHistoryToLetterboxdCsv;

internal class HtmlParser
{
    private readonly ProcessorConfig _processorConfig;
    private readonly string _exportFolder;
    private readonly CommandLineOptions _options;

    public HtmlParser(IOptions<HtmlHistoryToLetterboxdConfig> config, CommandLineOptions options)
    {
        _options = options;
        _exportFolder = config.Value.ExportFolder;
        _processorConfig = config.Value.Processors[_options.HistoryType.ToString()];
    }

    public void Parse()
    {
        var doc = new HtmlDocument();
        doc.Load(_options.FileName);

        var node = doc.DocumentNode.SelectSingleNode(_processorConfig.XPaths["ItemList"]);
        Console.WriteLine($"{node.ChildNodes.Count} nodes");

        var items = node.SelectNodes(_processorConfig.XPaths["Items"]);
        Console.WriteLine($"{items.Count} items");

        List<LetterboxdMovie> records = GetMovieRecords(items);

        ProcessPotentialTv(ref records);

        using var writer = new StreamWriter($"{_exportFolder}{_options.HistoryType}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<LetterboxdMovieMap>();

        csv.WriteRecords(records);
    }

    /// <summary>
    /// Items with "Season" in the name could be a DVD of Tv.  Applies to DVD shipping services like Netflix
    /// </summary>
    /// <param name="records"></param>
    private static void ProcessPotentialTv(ref List<LetterboxdMovie> records)
    {
        var recordsWithSeasonInTitle = records.Where(m => m.Title!.Contains("Season")).ToArray();
        if (recordsWithSeasonInTitle.Any())
        {
            Console.WriteLine("WARNING: The following records have \"Season\" in the title, they may be TV series.");
            Console.WriteLine();

            foreach (LetterboxdMovie letterboxdMovie in recordsWithSeasonInTitle)
            {
                Console.WriteLine(letterboxdMovie.Title);
            }

            Console.WriteLine();
            Console.Write("Do you want to include them in the CSV (y/n)?  ");

            var response = Console.ReadKey(true);
            var answers = new[] { ConsoleKey.Y, ConsoleKey.N };
            while (!answers.Contains(response.Key))
            {
                response = Console.ReadKey(true);
            }

            if (response.Key == ConsoleKey.N)
            {
                records = records.Except(recordsWithSeasonInTitle).ToList();
            }
        }
    }

    private List<LetterboxdMovie> GetMovieRecords(HtmlNodeCollection items)
    {
        var records = new List<LetterboxdMovie>();
        foreach (var item in items.Where(i => !string.IsNullOrEmpty(i.InnerText)))
        {
            // Get the watched date, if it exists
            DateTime? watchedDate = null;

            HtmlNode dateItemContext = item;
            if (_processorConfig.XPaths.TryGetValue("DateSelectorAncestorName", out var ancestorSelector) && !string.IsNullOrEmpty(ancestorSelector))
            {
                dateItemContext = item.Ancestors(_processorConfig.XPaths["DateSelectorAncestorName"]).First();
            }
            var watchedDateNode = dateItemContext.SelectSingleNode(_processorConfig.XPaths["DateSelector"]);
            if (watchedDateNode != null)
            {
                watchedDate = DateTime.ParseExact(watchedDateNode.InnerText, _processorConfig.WatchedDateFormat, CultureInfo.InvariantCulture).AddDays(_processorConfig.WatchedDateAddDays);
            }

            var movie = new LetterboxdMovie
            {
                WatchedDate = watchedDate?.ToString("yyyy-MM-dd"),
                Title = item.InnerText,
                Tags = _processorConfig.Tags
            };

            records.Add(movie);
        }

        return records;
    }
}