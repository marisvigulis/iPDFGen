using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Extensions;
using iPDFGen.Core.Models;

namespace iPDFGen.Core;

public sealed class PdfGenMetrics: IPdfGenMetrics
{
    private int _activeRequests;
    private int _available;
    private readonly int _max;
    private long _failedRequests;
    private long _totalRequests;
    private long _completedRequests;

    public PdfGenMetrics(PdfGenRegistrationSettings options)
    {
        _max = options.MaxDegreeOfParallelism;
    }

    public ValueTask<PdfGenUsage> GetUsageAsync()
    {
        var failedRequests = Interlocked.Read(ref _failedRequests);
        var totalRequests = Interlocked.Read(ref _totalRequests);
        var completed = Interlocked.Read(ref _completedRequests);

        var usageModel = new PdfGenUsage(
            Available: _available,
            Used: _activeRequests,
            Max: _max,
            TotalProcessed: completed,
            TotalFailed: failedRequests,
            TotalRequests: totalRequests
        );
        return ValueTask.FromResult(usageModel);
    }

    public void Start()
    {
        Interlocked.Increment(ref _activeRequests);
        Interlocked.Increment(ref _totalRequests);
        Interlocked.Decrement(ref _available);
    }

    public void Fail()
    {
        Interlocked.Increment(ref _available);
        Interlocked.Increment(ref _failedRequests);
        Interlocked.Decrement(ref _activeRequests);
    }

    public void Success()
    {
        Interlocked.Increment(ref _available);
        Interlocked.Increment(ref _completedRequests);
        Interlocked.Decrement(ref _activeRequests);
    }
}