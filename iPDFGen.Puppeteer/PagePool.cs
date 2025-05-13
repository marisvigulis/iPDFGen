using System.Collections.Concurrent;
using iPDFGen.Core;
using PuppeteerSharp;

namespace iPDFGen.Puppeteer;

internal sealed class PagePool : IAsyncDisposable
{
    private IBrowser? _browser;
    private readonly BlockingCollection<IPage> _pages = new();
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    internal async ValueTask Initialize()
    {
        BrowserFetcher browserFetcher = new BrowserFetcher
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
        var pageTasks = new int[PdfGenDefaults.MaxDegreeOfParallelism]
            .Select(_ => _browser.NewPageAsync());
        var pages = await Task.WhenAll(pageTasks);
        foreach (var page in pages)
        {
            _pages.Add(page);
        }
    }

    public async ValueTask<Stream> Run(Func<IPage, Task<Stream>> func)
    {
        await EnsureInitialized();
        var page = _pages.Take();
        var result = await func(page);
        _pages.Add(page);
        return result;
    }

    private async ValueTask EnsureInitialized()
    {
        if (_browser is not null)
        {
            return;
        }

        await Semaphore.WaitAsync();
        if (_browser is null)
        {
            await Initialize();
        }
        Semaphore.Release();
    }

    public void Dispose()
    {

        _browser?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser != null)
        {
            await _browser.CloseAsync();
        }
    }
}