using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.Core.Extensions;

public class PdfGenOptions
{
    public IServiceCollection ServiceCollection { get; }
    public TimeSpan Timeout { get; internal set; } = PdfGenDefaults.DefaultTimeout;
    public int MaxDegreeOfParallelism { get; internal set; } = PdfGenDefaults.MaxDegreeOfParallelism;

    public PdfGenOptions(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
    }

    public PdfGenOptions SetDefaultTimeout(TimeSpan timestamp)
    {
        Timeout = timestamp;
        return this;
    }

    public PdfGenOptions SetMaxDegreeOfParallelism(int degreeOfParallelism)
    {
        MaxDegreeOfParallelism = degreeOfParallelism;
        return this;
    }
}