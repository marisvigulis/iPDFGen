using System.Threading.Channels;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Models;
using OneOf;

namespace iPDFGen.Core;

public interface IGeneratorPool<out T>
{
    ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> RunAsync(
        Func<T, CancellationToken, ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>>> func,
        CancellationToken cancellationToken = default);

    ValueTask InitializeAsync();
}

public abstract class GeneratorPool<T> : IDisposable, IGeneratorPool<T>
{
    private readonly Channel<T> _processingUnits;
    private volatile bool _isInitialized;
    private readonly PdfGenRegistrationSettings _pdfGenOptions;
    private readonly SemaphoreSlim _initSemaphore = new(1, 1);
    private readonly IPdfGenMetrics _genMetrics;

    protected GeneratorPool(PdfGenRegistrationSettings pdfGenOptions, IPdfGenMetrics genMetrics)
    {
        _pdfGenOptions = pdfGenOptions;
        _genMetrics = genMetrics;
        _processingUnits = Channel.CreateBounded<T>(pdfGenOptions.MaxDegreeOfParallelism);
    }

    public async ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> RunAsync(
        Func<T, CancellationToken, ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>>> func,
        CancellationToken cancellationToken = default)
    {

        await EnsureInitializedAsync();
        var processingUnit = await _processingUnits.Reader.ReadAsync(cancellationToken);

        _genMetrics.Start();
        try
        {
            var result = await func(processingUnit, cancellationToken);

            if (result.IsT0)
            {
                _genMetrics.Success();
            }
            else
            {
                _genMetrics.Fail();
            }

            return result;
        }
        catch (OperationCanceledException)
        {
            _genMetrics.Fail();
            return new PdfGenErrorResult("0001", "Operation was cancelled");
        }
        catch (Exception ex)
        {
            _genMetrics.Fail();
            return new PdfGenErrorResult("0000", $"GeneratorPool internal error: {ex}");
        }
        finally
        {
            await _processingUnits.Writer.WriteAsync(processingUnit, CancellationToken.None);
        }
    }

    public virtual async ValueTask InitializeAsync()
    {
        if (!_isInitialized)
        {
            var processingUnits = await InitializeProcessingUnitsAsync(_pdfGenOptions);
            foreach (var processingUnit in processingUnits)
            {
                await _processingUnits.Writer.WriteAsync(processingUnit);
            }

            _isInitialized = true;
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
            await InitializeAsync();
        }
        finally
        {
            _initSemaphore.Release();
        }
    }

    /// <summary>
    /// This method shall return processing units, will get called once per application lifecycle
    /// </summary>
    /// <returns></returns>
    protected abstract ValueTask<T[]> InitializeProcessingUnitsAsync(PdfGenRegistrationSettings options);

    public virtual void Dispose()
    {
        _processingUnits.Writer.Complete();
        _initSemaphore.Dispose();
    }
}