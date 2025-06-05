using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Extensions;
using iPDFGen.RemoteServer.Models;
using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.RemoteServer.Extensions;

public static class RemoteServerPdfGenOptionsExtensions
{
    public static PdfGenOptions UseRemoteServer(this PdfGenOptions options, RemoteServerSettings settings)
    {
        options.ServiceCollection.AddSingleton<IPdfGenInitializer, RemoteServerInitializer>();
        options.ServiceCollection.AddSingleton<IPdfGenerator, RemoteServerGenerator>();
        options.ServiceCollection.AddSingleton<IGeneratorPool<bool>, RemoteServerGeneratorPool>();
        options.ServiceCollection.AddSingleton(settings);

        return options;
    }
}