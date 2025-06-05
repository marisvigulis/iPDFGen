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
    private readonly IGeneratorPool<IPage> _pagePool;

    public PlaywrightGenerator(IGeneratorPool<IPage> pagePool)
    {
        _pagePool = pagePool;
    }

    public ValueTask<UsageModel> UsageAsync() => _pagePool.UsageAsync();

    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> Generate(string markup, PdfGeneratorSettings? settings = null)
    {
        return _pagePool.RunAsync(async (page, _) =>
        {
            await page.SetContentAsync(markup, new PageSetContentOptions
            {
                Timeout = settings?.Timeout ?? PdfGenDefaults.DefaultTimeout.Milliseconds,

            });


            var fileBytes = await page.PdfAsync(settings?.ToPlaywrightPdfOptions());
            return fileBytes.Length == 0
                ? new PdfGenErrorResult("0002", "Playwright returned empty PDF")
                : new PdfGenSuccessResult
                {
                    Stream = new MemoryStream(fileBytes)
                };
        });
    }

    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateByUrl(string url, PdfGeneratorSettings? settings = null)
    {
        return _pagePool.RunAsync(async (page, _) =>
        {
            await page.GotoAsync(url, new PageGotoOptions
            {
                Timeout = settings?.Timeout ?? PdfGenDefaults.DefaultTimeout.Milliseconds,
            });


            var fileBytes = await page.PdfAsync(settings?.ToPlaywrightPdfOptions());
            return fileBytes.Length == 0
                ? new PdfGenErrorResult("0002", "Playwright returned empty PDF")
                : new PdfGenSuccessResult
                {
                    Stream = new MemoryStream(fileBytes)
                };
        });
    }
}