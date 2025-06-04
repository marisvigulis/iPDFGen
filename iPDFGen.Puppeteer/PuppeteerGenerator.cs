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
    private readonly PagePool _pagePool;

    public PuppeteerGenerator(PagePool pagePool)
    {
        _pagePool = pagePool;
    }

    public async ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> Generate(string markup, PdfGeneratorSettings? settings = null)
    {
        var pdfStream = await _pagePool.Run(async page =>
        {
            await page.SetContentAsync(markup, new NavigationOptions
            {
                Timeout = settings?.Timeout ?? PdfGenDefaults.DefaultTimeout.Milliseconds
            });

            return await page.PdfStreamAsync(settings.ToPuppeteerPdfOptions());
        });

        return new PdfGenSuccessResult
        {
            Stream = pdfStream
        };
    }

    public async ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateByUrl(string url, PdfGeneratorSettings? settings = null)
    {
        var pdfStream = await _pagePool.Run(async page =>
        {
            await page.GoToAsync(url, new NavigationOptions
            {
                Timeout = settings?.Timeout
            });

            await page.EmulateMediaTypeAsync(MediaType.Screen);

            var result = await page.PdfStreamAsync(settings.ToPuppeteerPdfOptions());

            return result;
        });

        if (pdfStream is null)
        {
            return new PdfGenErrorResult("01", "Internal error");
        }

        return new PdfGenSuccessResult
        {
            Stream = pdfStream
        };
    }
}