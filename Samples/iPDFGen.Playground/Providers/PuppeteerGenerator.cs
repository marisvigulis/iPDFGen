using iPDFGen.Puppeteer.Extensions;

namespace iPDFGen.Playground.Providers;

public class PuppeteerGenerator : GeneratorBase, IAsyncDisposable
{
    public ValueTask Setup()
    {
        return SetupInternal(a => a.UsePuppeteerPdfGen());
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