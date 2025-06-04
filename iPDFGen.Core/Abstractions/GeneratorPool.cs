using System.Threading.Channels;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Models;
using OneOf;

namespace iPDFGen.Core.Abstractions;

public interface IGeneratorPool<out T>
{
    ValueTask<UsageModel> Usage();

    ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> RunAsync(
        Func<T, CancellationToken, Task<OneOf<PdfGenSuccessResult, PdfGenErrorResult>>> func,
        CancellationToken cancellationToken = default);
}

public abstract class GeneratorPool<T> : IDisposable, IGeneratorPool<T>
{
    private readonly Channel<T> _processingUnits;
    private volatile bool _isInitialized;
    private readonly PdfGenRegistrationSettings _pdfGenOptions;
    private readonly SemaphoreSlim _initSemaphore = new(1, 1);
    private long _totalRequests;
    private long _activeRequests;
    private long _completedRequests;
    private long _failedRequests;

    protected GeneratorPool(PdfGenRegistrationSettings pdfGenOptions)
    {
        _pdfGenOptions = pdfGenOptions;
        _processingUnits = Channel.CreateBounded<T>(pdfGenOptions.MaxDegreeOfParallelism);
    }

    public virtual ValueTask<UsageModel> Usage()
    {
        var active = Interlocked.Read(ref _activeRequests);
        var total = Interlocked.Read(ref _totalRequests);
        var completed = Interlocked.Read(ref _completedRequests);
        var available = _processingUnits.Reader.CanCount
            ? _processingUnits.Reader.Count
            : 0;

        var usageModel = new UsageModel(
            Available: available,
            Used: active,
            Max: _pdfGenOptions.MaxDegreeOfParallelism,
            TotalProcessed: completed,
            TotalFailed: _failedRequests,
            TotalRequests: total
        );
        return ValueTask.FromResult(usageModel);
    }

    public async ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> RunAsync(
        Func<T, CancellationToken, Task<OneOf<PdfGenSuccessResult, PdfGenErrorResult>>> func,
        CancellationToken cancellationToken = default)
    {
        Interlocked.Increment(ref _totalRequests);
        await EnsureInitializedAsync();
        var processingUnit = await _processingUnits.Reader.ReadAsync(cancellationToken);

        Interlocked.Increment(ref _activeRequests);
        try
        {
            var result = await func(processingUnit, cancellationToken);

            result.Match(
                _ => Interlocked.Increment(ref _completedRequests),
                _ => Interlocked.Increment(ref _failedRequests)
            );
            return result;
        }
        catch (OperationCanceledException cancelledException)
        {
            return new PdfGenErrorResult("0001", "Operation was cancelled");
        }
        catch (Exception ex)
        {
            Interlocked.Increment(ref _failedRequests);
            return new PdfGenErrorResult("0000", $"GeneratorPool internal error: {ex}");
        }
        finally
        {
            Interlocked.Decrement(ref _activeRequests);
            await _processingUnits.Writer.WriteAsync(processingUnit, CancellationToken.None);
        }
    }

    private async ValueTask EnsureInitializedAsync()
    {
        if (_isInitialized)
        {
            return;
        }

        await _initSemaphore.WaitAsync();
        try
        {
            if (!_isInitialized)
            {
                var processingUnits = await InitializeAsync(_pdfGenOptions);
                foreach (var processingUnit in processingUnits)
                {
                    await _processingUnits.Writer.WriteAsync(processingUnit);
                }

                _isInitialized = true;
            }
        }
        finally
        {
            _initSemaphore.Release();
        }
    }

    /// <summary>
    /// This method shall initialize _processingUnits
    /// </summary>
    /// <returns></returns>
    protected abstract ValueTask<T[]> InitializeAsync(PdfGenRegistrationSettings options);

    public abstract void Dispose();
}