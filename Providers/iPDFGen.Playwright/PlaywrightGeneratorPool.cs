using iPDFGen.Core;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Models;
using Microsoft.Playwright;

namespace iPDFGen.Playwright;

internal sealed class PlaywrightGeneratorPool : GeneratorPool<IPage>
{
    private IPlaywright? _playwright;


    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public PlaywrightGeneratorPool(PdfGenRegistrationSettings pdfGenOptions, IPdfGenMetrics metrics) : base(pdfGenOptions, metrics)
    {
    }

    protected override async ValueTask<IPage[]> InitializeProcessingUnitsAsync(PdfGenRegistrationSettings options)
    {
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Program.Main(["install", "chromium"]);
        IBrowser browser = await _playwright.Chromium.LaunchAsync(new()
        {
            Headless = true
        });
        IBrowserContext context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            JavaScriptEnabled = false
        });
        var pageTasks = new int[options.MaxDegreeOfParallelism]
            .Select(_ => context.NewPageAsync());
        return await Task.WhenAll(pageTasks);
    }

    public override void Dispose()
    {
        base.Dispose();
        _playwright?.Dispose();
    }
}