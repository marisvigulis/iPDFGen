using iPDFGen.Core.Models;

namespace iPDFGen.Core.Abstractions;

public interface IPdfGenMetrics
{
    ValueTask<PdfGenUsage> GetUsageAsync();
    void Success();
    void Fail();
    void Start();
}