using iPDFGen.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPdfGen(this IServiceCollection serviceCollection, Action<PdfGenOptions> registerOptions)
    {
        var options = new PdfGenOptions(serviceCollection);
        registerOptions(options);
        serviceCollection.AddSingleton<PdfGenRegistrationSettings>(_ => options.ToRegistrationSettings());
        return serviceCollection;
    }
}