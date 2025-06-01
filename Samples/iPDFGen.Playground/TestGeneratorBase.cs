using System.Reflection;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.Playground;

public abstract class TestGeneratorBase
{
    private ServiceProvider _provider = null!;
    private Dictionary<string, string> _markups = null!;

    public async Task SetupInternal(Action<PdfGenOptions> register, string defaultTemplateName = "resume.A4.xs.html")
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddPdfGen(s =>
            {
                register(s.SetDefaultTimeout(TimeSpan.FromSeconds(30)));
            });

        _provider = serviceCollection.BuildServiceProvider();

        await _provider
            .GetRequiredService<IPdfGenInitializer>()
            .Initialize();

         await LoadEmbeddedResources();
    }

    protected async Task<Stream> GenerateByMarkup(string markup)
    {
        var pdfStream = await _provider
            .GetRequiredService<IPdfGenerator>()
            .Generate(markup);

        var result =
            pdfStream.Match<PdfGenSuccessResult>(
                result => result,
                _ => throw new Exception("Failed to generate PDF")
            );
        return result.Stream;
    }

    protected async ValueTask<Stream> GenerateByUrl(string url = "https://docraptor.com/templates/resume/resume.A4.html")
    {
        var pdfStream = await _provider
            .GetRequiredService<IPdfGenerator>()
            .GenerateByUrl(url);

        var result =
            pdfStream.Match<PdfGenSuccessResult>(
                result => result,
                _ => throw new Exception("Failed to generate PDF")
            );
        return result.Stream;
    }

    private async ValueTask LoadEmbeddedResources()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var embeddedResourceNames = assembly.GetManifestResourceNames()
            .Where(e => e.StartsWith("iPDFGen.Playground.Templates"));
        List<KeyValuePair<string, string>> templates = new List<KeyValuePair<string, string>>();
        foreach (var embeddedResourceName in embeddedResourceNames)
        {
            await using var stream = assembly.GetManifestResourceStream(embeddedResourceName)!;
            using var reader = new StreamReader(stream);
            var content =  await reader.ReadToEndAsync();
            templates.Add(new KeyValuePair<string, string>(embeddedResourceName, content));
        }

        _markups = templates.ToDictionary(t => t.Key, t => t.Value);
    }

    public ValueTask DisposeAsync()
    {
        return _provider.DisposeAsync();
    }
}