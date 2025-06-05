using iPDFGen.Core;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Models;

namespace iPDFGen.RemoteServer;

public class RemoteServerGeneratorPool: GeneratorPool<bool>
{
    public RemoteServerGeneratorPool(PdfGenRegistrationSettings pdfGenOptions, IPdfGenMetrics metrics)
        : base(pdfGenOptions, metrics)
    {
    }

    protected override ValueTask<bool[]> InitializeProcessingUnitsAsync(PdfGenRegistrationSettings options)
    {
        return ValueTask.FromResult(Enumerable.Range(0, options.MaxDegreeOfParallelism)
            .Select(e => true)
            .ToArray());
    }
}