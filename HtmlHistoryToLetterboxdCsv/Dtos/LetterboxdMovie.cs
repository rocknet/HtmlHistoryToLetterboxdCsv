using CsvHelper.Configuration;

namespace HtmlHistoryToLetterboxdCsv.Dtos;

internal class LetterboxdMovie
{
    /// <summary>
    /// String (optional), the boxd.it URI of the matching film or diary entry, example: https://boxd.it/29qU
    /// Note: letterboxd.com URIs are also supported for backwards compatibility.
    /// </summary>
    public string? LetterboxdUri { get; set; }

    /// <summary>
    /// Number (optional), example: 860
    /// </summary>
    public int? TmdbId { get; set; }

    /// <summary>
    /// String (optional), example: tt0086567
    /// </summary>
    public string? ImdbId { get; set; }

    /// <summary>
    /// String (optional), used for matching when no ID or URI is provided
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// YYYY (optional), used for matching when no ID or URI is provided
    /// </summary>
    public int? Year { get; set; }

    /// <summary>
    /// String (optional), used for matching when no ID or URI is provided; multiple entries should be comma-separated
    /// </summary>
    public string? Directors { get; set; }

    /// <summary>
    /// Number (optional, decimals from 0.5–5 including 0.5 increments), rating out of 5
    /// </summary>
    public decimal? Rating { get; set; }

    /// <summary>
    /// Number (optional, integers from 1–10), rating out of 10 (will be converted to 0.5–5 scale)
    /// </summary>
    public int? Rating10 { get; set; }

    /// <summary>
    /// YYYY-MM-DD (optional), creates a Diary Entry for the film on this day
    /// </summary>
    public string? WatchedDate { get; set; }

    /// <summary>
    /// Boolean (optional), if true, sets the rewatch flag on the Diary Entry when WatchedDate is provided
    /// </summary>
    public bool? Rewatch { get; set; }

    /// <summary>
    /// String (optional), added to Diary Entry when WatchedDate is provided; multiple entries should be comma-separated
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Text/HTML (optional), allows the same HTML tags as the website, added to Diary Entry when WatchedDate is provided, otherwise added as a review with no specified date*
    /// This column header can also be used when importing to a list, to populate the Notes field.
    /// </summary>
    public string? Review { get; set; }
}

internal sealed class LetterboxdMovieMap : ClassMap<LetterboxdMovie>
{
    public LetterboxdMovieMap()
    {
        Map(m => m.LetterboxdUri).Index(0).Name("LetterboxdURI");
        Map(m => m.TmdbId).Index(1).Name("tmdbID");
        Map(m => m.ImdbId).Index(2).Name("imdbID");
        Map(m => m.Title).Index(3);
        Map(m => m.Year).Index(4);
        Map(m => m.Directors).Index(5);
        Map(m => m.Rating).Index(6);
        Map(m => m.Rating10).Index(7);
        Map(m => m.WatchedDate).Index(8);
        Map(m => m.Rewatch).Index(9);
        Map(m => m.Tags).Index(10);
        Map(m => m.Review).Index(11);
    }
}