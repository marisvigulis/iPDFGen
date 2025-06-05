using iPDFGen.Puppeteer.Extensions;

namespace iPDFGen.Playground.Providers;

public sealed class PuppeteerGenerator : GeneratorBase
{
    public ValueTask Setup()
    {
        return SetupInternal(options => options.UsePuppeteer());
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