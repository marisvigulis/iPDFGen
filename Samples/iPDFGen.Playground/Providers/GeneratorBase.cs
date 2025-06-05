using System.Reflection;
using iPDFGen.Core;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.Playground.Providers;

public abstract class GeneratorBase: IDisposable
{
    private ServiceProvider _provider = null!;
    private Dictionary<string, string> _markups = null!;
    private string _defaultTemplate = null!;
    private const string DefaultTemplateUrl = "https://raw.githubusercontent.com/marisvigulis/iPDFGen/refs/heads/main/Samples/iPDFGen.Playground/Templates/resume.A4.xs.html";

    protected async ValueTask SetupInternal(Action<PdfGenOptions> register)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddPdfGen(s =>
            {
                register(s
                    .SetDefaultTimeout(TimeSpan.FromSeconds(30))
                    .SetMaxDegreeOfParallelism(PdfGenDefaults.MaxDegreeOfParallelism));
            });

        _provider = serviceCollection.BuildServiceProvider();

        await _provider
            .GetRequiredService<IPdfGenInitializer>()
            .InitializeAsync();

         await LoadEmbeddedResources();
    }

    protected async ValueTask<Stream> GenerateByMarkup(string? markup)
    {
        var pdfStream = await _provider
            .GetRequiredService<IPdfGenerator>()
            .Generate(markup ?? _defaultTemplate);

        var result =
            pdfStream.Match<PdfGenSuccessResult>(
                result => result,
                err => throw new Exception($"Failed to generate PDF, {err.Code}, {err.Message}")
            );
        return result.Stream;
    }

    protected async ValueTask<Stream> GenerateByUrl(string url = DefaultTemplateUrl)
    {
        var pdfStream = await _provider
            .GetRequiredService<IPdfGenerator>()
            .GenerateByUrl(url);

        var result =
            pdfStream.Match<PdfGenSuccessResult>(
                result => result,
                err => throw new Exception($"Failed to generate PDF, {err.Code}, {err.Message}")
            );
        return result.Stream;
    }

    private async ValueTask LoadEmbeddedResources()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var embeddedResourceNames = assembly.GetManifestResourceNames()
            .Where(e => e.StartsWith("iPDFGen.Playground.Templates"));
        List<KeyValuePair<string, string>> templates = [];
        foreach (var embeddedResourceName in embeddedResourceNames)
        {
            await using var stream = assembly.GetManifestResourceStream(embeddedResourceName)!;
            using var reader = new StreamReader(stream);
            var content =  await reader.ReadToEndAsync();
            templates.Add(new KeyValuePair<string, string>(embeddedResourceName, content));
        }

        _markups = templates.ToDictionary(t => t.Key, t => t.Value);

        _defaultTemplate = _markups.First().Value;
    }

    public void Dispose()
    {
        _provider.Dispose();
    }
}