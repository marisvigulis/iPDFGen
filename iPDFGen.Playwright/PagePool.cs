using System.Collections.Concurrent;
using iPDFGen.Core;
using Microsoft.Playwright;

namespace iPDFGen.Playwright;

public class PagePool: IDisposable
{
    private IPlaywright? _playwright;
    private readonly BlockingCollection<IPage> _pages = new();
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    internal async ValueTask Initialize()
    {
        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });
        // await _playwright.Chromium.InstallAsync(); // Installs Chromium specifically
        var browser = await _playwright.Chromium.LaunchAsync(new ()
        {
            Headless = true
        });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions
        {
            JavaScriptEnabled = false
        });
        var pageTasks = new int[PdfGenDefaults.MaxDegreeOfParallelism]
            .Select(_ => context.NewPageAsync());
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
        if (_playwright is not null)
        {
            return;
        }

        await Semaphore.WaitAsync();
        if (_playwright is null)
        {
            await Initialize();
        }
        Semaphore.Release();
    }

    public void Dispose()
    {
        if (_playwright != null)
        {
            _playwright.Dispose();
        }
    }
}