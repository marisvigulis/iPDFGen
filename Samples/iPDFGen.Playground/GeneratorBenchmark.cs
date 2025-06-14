using BenchmarkDotNet.Attributes;
using iPDFGen.Core;
using iPDFGen.Playground.Providers;

namespace iPDFGen.Playground;

[ThreadingDiagnoser]
[MemoryDiagnoser]
public class GeneratorBenchmark
{
    private PuppeteerGenerator _puppeteerGenerator = null!;
    private PlaywrightGenerator _playwrightGenerator = null!;
    private RemoteServerGenerator _remoteServerGenerator = null!;
    private const int IterationsPerThread = 5;
    private static readonly int Iterations = PdfGenDefaults.MaxDegreeOfParallelism * IterationsPerThread;

    [GlobalSetup]
    public async ValueTask Setup()
    {
        _puppeteerGenerator = new PuppeteerGenerator();
        await _puppeteerGenerator.Setup();

        _playwrightGenerator = new PlaywrightGenerator();
        await _playwrightGenerator.Setup();

        _remoteServerGenerator = new RemoteServerGenerator();
        await _remoteServerGenerator.Setup();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _puppeteerGenerator.Dispose();
        _playwrightGenerator.Dispose();
        _remoteServerGenerator.Dispose();
    }

    [Benchmark]
    public async ValueTask PuppeteerSingle()
    {
        await using var stream = await _puppeteerGenerator.Generate();
        await stream.ReadExactlyAsync(new byte[stream.Length]);
    }

    [Benchmark]
    public async ValueTask PlaywrightSingle()
    {
        await using var stream = await _playwrightGenerator.Generate();
        await stream.ReadExactlyAsync(new byte[stream.Length]);
    }

    [Benchmark]
    public async ValueTask RemoteServerSingle()
    {
        await using var stream = await _remoteServerGenerator.Generate();
        await stream.ReadExactlyAsync(new byte[stream.Length]);
    }


    [Benchmark]
    public async ValueTask PuppeteerSingleByUrl()
    {
        await using var stream = await _puppeteerGenerator.GenerateByUrl();
        await stream.ReadExactlyAsync(new byte[stream.Length]);
    }

    [Benchmark]
    public async ValueTask PlaywrightSingleByUrl()
    {
        await using var stream = await _playwrightGenerator.GenerateByUrl();
        await stream.ReadExactlyAsync(new byte[stream.Length]);
    }

    [Benchmark]
    public async ValueTask RemoteServerSingleByUrl()
    {
        await using var stream = await _remoteServerGenerator.GenerateByUrl();
        await stream.ReadExactlyAsync(new byte[stream.Length]);
    }

    [Benchmark]
    public async ValueTask PuppeteerEighty()
    {
        await Parallel.ForEachAsync(new int[Iterations],
            new ParallelOptions
            {
                MaxDegreeOfParallelism = PdfGenDefaults.MaxDegreeOfParallelism
            },
            async (_, _) => await PuppeteerSingle());
    }

    [Benchmark]
    public async ValueTask PlaywrightEighty()
    {
        await Parallel.ForEachAsync(new int[Iterations],
            new ParallelOptions
            {
                MaxDegreeOfParallelism = PdfGenDefaults.MaxDegreeOfParallelism
            },
            async (_, _) => await PlaywrightSingle());
    }

    [Benchmark]
    public async ValueTask RemoteServerEighty()
    {
        await Parallel.ForEachAsync(new int[Iterations],
            new ParallelOptions
            {
                MaxDegreeOfParallelism = PdfGenDefaults.MaxDegreeOfParallelism
            },
            async (_, _) => await RemoteServerSingle());
    }
}