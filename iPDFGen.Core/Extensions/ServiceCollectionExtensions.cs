using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPdfGen(this IServiceCollection serviceCollection, Action<PdfGenOptions> registerOptions)
    {
        registerOptions(new PdfGenOptions(serviceCollection));
        return serviceCollection;
    }
}