using iPDFGen.Playwright.Extensions;

namespace iPDFGen.Playground.Providers;

public class PlaywrightGenerator : GeneratorBase, IAsyncDisposable
{
    public ValueTask Setup()
    {
        return SetupInternal(a => a.UsePlaywright());
    }

    public ValueTask<Stream> Generate()
    {
        return GenerateByMarkup(null);
    }

    public ValueTask<Stream> GenerateByUrl()
    {
        return base.GenerateByUrl();
    }
}