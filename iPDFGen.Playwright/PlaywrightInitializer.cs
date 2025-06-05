using iPDFGen.Core.Abstractions;
using Microsoft.Playwright;

namespace iPDFGen.Playwright;

internal sealed class PlaywrightInitializer: IPdfGenInitializer
{
    private readonly IGeneratorPool<IPage> _playwrightGeneratorPool;

    public PlaywrightInitializer(IGeneratorPool<IPage> playwrightGeneratorPool)
    {
        _playwrightGeneratorPool = playwrightGeneratorPool;
    }


    public async ValueTask InitializeAsync()
    {
        await _playwrightGeneratorPool.InitializeAsync();
    }
}