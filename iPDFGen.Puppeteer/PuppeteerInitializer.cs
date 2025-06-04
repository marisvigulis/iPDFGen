using iPDFGen.Core.Abstractions;

namespace iPDFGen.Puppeteer;

internal sealed class PuppeteerInitializer: IPdfGenInitializer
{
    private readonly PagePool _pagePool;

    public PuppeteerInitializer(PagePool pagePool)
    {
        _pagePool = pagePool;
    }

    public async ValueTask Initialize()
    {
        await _pagePool.Initialize();
    }
}