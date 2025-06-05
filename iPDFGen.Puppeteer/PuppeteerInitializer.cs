using iPDFGen.Core.Abstractions;
using PuppeteerSharp;

namespace iPDFGen.Puppeteer;

internal sealed class PuppeteerInitializer: IPdfGenInitializer
{
    private readonly IGeneratorPool<IPage> _pagePool;

    public PuppeteerInitializer(IGeneratorPool<IPage> pagePool)
    {
        _pagePool = pagePool;
    }

    public ValueTask InitializeAsync()
    {
        return _pagePool.InitializeAsync();
    }
}