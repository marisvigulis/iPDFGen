using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.Puppeteer.Extensions;

public static class PuppeteerPdfGenOptionsExtensions
{
    public static PdfGenOptions UsePuppeteer(this PdfGenOptions options)
    {
        options.ServiceCollection.AddSingleton<IPdfGenInitializer, PuppeteerInitializer>();
        options.ServiceCollection.AddSingleton<IPdfGenerator, PuppeteerGenerator>();
        options.ServiceCollection.AddSingleton<PagePool, PagePool>();

        return options;
    }
}