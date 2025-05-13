using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.Puppeteer.Extensions;

public static class PuppeteerPdfGenOptionsExtensions
{
    public static PdfGenOptions RegisterPuppeteerPdfGen(this PdfGenOptions options)
    {
        options.ServiceCollection.AddSingleton<IPdfGenInitializer, PuppeteerInitializer>();
        options.ServiceCollection.AddSingleton<IPdfGenerator, PuppeteerPdfGenerator>();
        options.ServiceCollection.AddSingleton<PagePool, PagePool>();

        return options;
    }
}