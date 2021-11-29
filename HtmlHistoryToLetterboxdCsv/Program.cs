using CommandLine;
using HtmlHistoryToLetterboxdCsv;
using HtmlHistoryToLetterboxdCsv.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Parser.Default.ParseArguments<CommandLineOptions>(args)
    .WithParsed(options =>
    {
        var host = Host.CreateDefaultBuilder()
        .ConfigureServices((hbc, services) =>
        {
            services.AddSingleton(options);
            services.AddScoped<HtmlParser>();
            services.Configure<HtmlHistoryToLetterboxdConfig>(hbc.Configuration.GetSection("HtmlHistoryToLetterboxdConfig"));
        })
        .Build();

        var app = ActivatorUtilities.CreateInstance<App>(host.Services);
        app.Run();
    });