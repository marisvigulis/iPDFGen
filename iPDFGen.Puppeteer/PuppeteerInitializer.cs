using iPDFGen.Core.Abstractions;

namespace iPDFGen.Puppeteer;

public class PuppeteerInitializer: IPdfGenInitializer
{
    private static readonly SemaphoreSlim Semaphore = new(1, 1);

    public Task Initialize()
    {
        throw new NotImplementedException();
    }

    public Task EnsureInitialized()
    {
        throw new NotImplementedException();
    }
}