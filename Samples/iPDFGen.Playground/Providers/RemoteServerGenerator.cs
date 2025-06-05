using iPDFGen.RemoteServer.Extensions;
using iPDFGen.RemoteServer.Models;
using iPDFGen.Server.Contracts;

namespace iPDFGen.Playground.Providers;

public sealed class RemoteServerGenerator : GeneratorBase
{
    public ValueTask Setup()
    {
        return SetupInternal(options => options
            .UseRemoteServer(new RemoteServerSettings
            {
                BaseUrl = "http://127.0.0.1:8080",
                CompressionType = CompressionType.None,
                SharedSecret = PdfGenServerConstants.DefaultSharedSecret
            }));
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