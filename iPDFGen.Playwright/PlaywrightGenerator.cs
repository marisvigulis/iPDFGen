using iPDFGen.Core;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Models;
using iPDFGen.Playwright.Extensions;
using Microsoft.Playwright;
using OneOf;

namespace iPDFGen.Playwright;

internal sealed class PlaywrightGenerator: IPdfGenerator
{
    private readonly PagePool _pagePool;

    public PlaywrightGenerator(PagePool pagePool)
    {
        _pagePool = pagePool;
    }

    public async ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> Generate(string markup, PdfGenSettings? settings = null)
    {
        var pdfStream = await _pagePool.Run(async page =>
        {
            await page.SetContentAsync(markup, new PageSetContentOptions
            {
                Timeout = settings?.Timeout ?? PdfGenDefaults.DefaultTimeout.Milliseconds
            });

            var fileBytes = await page.PdfAsync(settings?.ToPlaywrightPdfOptions());

            return new MemoryStream(fileBytes);
        });

        return new PdfGenSuccessResult
        {
            Stream = pdfStream
        };
    }

    public async ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateByUrl(string url, PdfGenSettings? settings = null)
    {
        var pdfStream = await _pagePool.Run(async page =>
        {
            await page.GotoAsync(url, new PageGotoOptions
            {
                Timeout = settings?.Timeout ?? PdfGenDefaults.DefaultTimeout.Milliseconds
            });

            var result = await page.PdfAsync(settings.ToPlaywrightPdfOptions());

            return new MemoryStream(result);
        });

        return new PdfGenSuccessResult
        {
            Stream = pdfStream
        };
    }
}