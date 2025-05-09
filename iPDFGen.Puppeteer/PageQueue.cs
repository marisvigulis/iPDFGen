using PuppeteerSharp;

namespace iPDFGen.Puppeteer;

internal static class PageQueue
{
    static async Task InitializeBrowser()
    {
        var browserFetcher = new BrowserFetcher
        {
            Browser = SupportedBrowser.Chromium
        };
        await browserFetcher.DownloadAsync();
    }
}