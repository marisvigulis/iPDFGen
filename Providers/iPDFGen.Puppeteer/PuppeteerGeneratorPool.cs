using iPDFGen.Core;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Models;
using PuppeteerSharp;

namespace iPDFGen.Puppeteer;

internal sealed class PuppeteerGeneratorPool : GeneratorPool<IPage>
{
    private IBrowser? _browser;

    public PuppeteerGeneratorPool(PdfGenRegistrationSettings pdfGenOptions, IPdfGenMetrics metrics) : base(pdfGenOptions, metrics)
    {
    }

    protected override async ValueTask<IPage[]> InitializeProcessingUnitsAsync(PdfGenRegistrationSettings options)
    {
        var browserFetcher = new BrowserFetcher
        {
            Browser = SupportedBrowser.Chromium
        };
        await browserFetcher.DownloadAsync();
        _browser = await PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            Browser = SupportedBrowser.Chromium,
            HeadlessMode = HeadlessMode.True
        });
        var pageTasks = new int[options.MaxDegreeOfParallelism]
            .Select(_ => _browser.NewPageAsync());
        return await Task.WhenAll(pageTasks);
    }

    public override void Dispose()
    {
        base.Dispose();
        _browser?.Dispose();
    }
}