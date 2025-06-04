using iPDFGen.Playwright.Extensions;

namespace iPDFGen.Playground.Providers;

public sealed class PlaywrightGenerator : GeneratorBase
{
    public ValueTask Setup()
    {
        return SetupInternal(options => options.UsePlaywright());
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