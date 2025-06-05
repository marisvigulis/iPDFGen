using iPDFGen.Core;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Models;
using iPDFGen.Puppeteer.Extensions;
using OneOf;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace iPDFGen.Puppeteer;

internal sealed class PuppeteerGenerator: IPdfGenerator
{
    private readonly IGeneratorPool<IPage> _pagePool;

    public PuppeteerGenerator(IGeneratorPool<IPage> pagePool)
    {
        _pagePool = pagePool;
    }

    public async ValueTask<OneOf<UsageModel, PdfGenErrorResult>> UsageAsync() => await _pagePool.UsageAsync();

    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateAsync(string markup, PdfGeneratorSettings? settings = null)
    {
        return _pagePool.RunAsync(async (page, _) =>
        {
            await page.SetContentAsync(markup, new NavigationOptions
            {
                Timeout = settings?.Timeout ?? PdfGenDefaults.DefaultTimeout.Milliseconds
            });

            await page.EmulateMediaTypeAsync(MediaType.Screen);

            return new PdfGenSuccessResult
            {
                Stream = await page.PdfStreamAsync(settings.ToPuppeteerPdfOptions())
            };
        });
    }

    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateByUrlAsync(string url, PdfGeneratorSettings? settings = null)
    {
        return _pagePool.RunAsync(async (page, _) =>
        {
            await page.GoToAsync(url, new NavigationOptions
            {
                Timeout = settings?.Timeout
            });

            await page.EmulateMediaTypeAsync(MediaType.Screen);

            return new PdfGenSuccessResult
            {
                Stream = await page.PdfStreamAsync(settings.ToPuppeteerPdfOptions())
            };
        });
    }
}