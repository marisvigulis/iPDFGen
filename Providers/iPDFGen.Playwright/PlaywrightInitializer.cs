using iPDFGen.Core;
using iPDFGen.Core.Abstractions;
using Microsoft.Playwright;

namespace iPDFGen.Playwright;

internal sealed class PlaywrightInitializer: IPdfGenInitializer
{
    private readonly IGeneratorPool<IPage> _generatorPool;

    public PlaywrightInitializer(IGeneratorPool<IPage> generatorPool)
    {
        _generatorPool = generatorPool;
    }


    public async ValueTask InitializeAsync()
    {
        await _generatorPool.InitializeAsync();
    }
}