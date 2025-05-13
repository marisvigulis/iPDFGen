using Microsoft.Extensions.DependencyInjection;

namespace iPDFGen.Core.Extensions;

public class PdfGenOptions
{
    public IServiceCollection ServiceCollection { get; }
    public double Timeout { get; internal set; } = PdfGenDefaults.DefaultTimeout;
    public int MaxDegreeOfParallelism { get; internal set; } = PdfGenDefaults.MaxDegreeOfParallelism;

    public PdfGenOptions(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
    }

    public PdfGenOptions SetDefaultTimeout(TimeSpan timestamp)
    {
        Timeout = timestamp.TotalMilliseconds;
        return this;
    }

    public PdfGenOptions SetMaxDegreeOfParallelism(int degreeOfParallelism)
    {
        MaxDegreeOfParallelism = degreeOfParallelism;
        return this;
    }
}