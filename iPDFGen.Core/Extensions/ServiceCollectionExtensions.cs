using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPdfGen(this IServiceCollection serviceCollection, Action<PdfGenOptions> registerOptions)
    {
        var options = new PdfGenOptions(serviceCollection);
        serviceCollection.AddSingleton<PdfGenRegistrationSettings>(_ => options.ToRegistrationSettings());
        serviceCollection.AddSingleton<IPdfGenMetrics, PdfGenMetrics>();
        registerOptions(options);
        return serviceCollection;
    }
}