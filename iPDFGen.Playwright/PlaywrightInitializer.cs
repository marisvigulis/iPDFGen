using iPDFGen.Core.Abstractions;

namespace iPDFGen.Playwright;

internal sealed class PlaywrightInitializer: IPdfGenInitializer
{
    private readonly PagePool _pagePool;

    public PlaywrightInitializer(PagePool pagePool)
    {
        _pagePool = pagePool;
    }

    public async ValueTask Initialize()
    {
        await _pagePool.Initialize();
    }
}