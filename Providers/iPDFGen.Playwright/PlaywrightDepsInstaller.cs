using Microsoft.Playwright;

namespace iPDFGen.Playwright;

public sealed class PlaywrightDepsInstaller
{
    public void Install()
    {
        Program.Main(["install-deps"]);
    }
}