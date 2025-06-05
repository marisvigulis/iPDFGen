using iPDFGen.Core;
using iPDFGen.Core.Abstractions;
using PuppeteerSharp;

namespace iPDFGen.Puppeteer;

internal sealed class PuppeteerInitializer: IPdfGenInitializer
{
    private readonly IGeneratorPool<IPage> _generatorPool;

    public PuppeteerInitializer(IGeneratorPool<IPage> generatorPool)
    {
        _generatorPool = generatorPool;
    }

    public ValueTask InitializeAsync()
    {
        return _generatorPool.InitializeAsync();
    }
}