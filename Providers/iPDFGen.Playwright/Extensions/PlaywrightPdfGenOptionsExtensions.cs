using iPDFGen.Core;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace iPDFGen.Playwright.Extensions;

public static class PlaywrightPdfGenOptionsExtensions
{
    public static PdfGenOptions UsePlaywright(this PdfGenOptions options)
    {
        options.ServiceCollection.AddSingleton<IPdfGenInitializer, PlaywrightInitializer>();
        options.ServiceCollection.AddSingleton<IPdfGenerator, PlaywrightGenerator>();
        options.ServiceCollection.AddSingleton<IGeneratorPool<IPage>, PlaywrightGeneratorPool>();

        return options;
    }
}