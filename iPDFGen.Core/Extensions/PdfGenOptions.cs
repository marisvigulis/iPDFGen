using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.Core.Extensions;

public class PdfGenOptions
{
    public IServiceCollection ServiceCollection { get; }

    public PdfGenOptions(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
    }
}