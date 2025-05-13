using System.Reflection;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Extensions;
using iPDFGen.Puppeteer.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.Playground;

public class TestGenerator: IAsyncDisposable
{
    private ServiceProvider _provider = null!;
    private string _testMarkup = null!;

    public async Task Setup(string templateName)
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddPdfGen(s =>
        {
            s.RegisterPuppeteerPdfGen();
        });

        _provider = serviceCollection.BuildServiceProvider();

        await _provider
            .GetRequiredService<IPdfGenInitializer>()
            .Initialize();

        _testMarkup = await ReadEmbeddedResource(templateName);
    }

    public async Task<Stream> Generate()
    {
        var pdfStream = await _provider
            .GetRequiredService<IPdfGenerator>()
            .Generate(_testMarkup);

        var result = pdfStream.Match<PdfGenSuccessResult>(result => result, _ => throw new Exception("Failed to generate PDF"));
        return result.Stream;
    }

    public async ValueTask<Stream> GenerateByUrl()
    {
        var pdfStream = await _provider
            .GetRequiredService<IPdfGenerator>()
            .GenerateByUrl("https://docraptor.com/templates/resume/resume.A4.html");

        var result = pdfStream.Match<PdfGenSuccessResult>(result => result, _ => throw new Exception("Failed to generate PDF"));
        return result.Stream;
    }

    private static async ValueTask<string> ReadEmbeddedResource(string templateName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var embeddedResourceNames = assembly.GetManifestResourceNames();
        var embeddedResourceName = embeddedResourceNames
            .Single(rn => rn.EndsWith(templateName, StringComparison.InvariantCultureIgnoreCase));
        await using var stream = assembly.GetManifestResourceStream(embeddedResourceName)!;
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    public ValueTask DisposeAsync()
    {
        return _provider.DisposeAsync();
    }
}