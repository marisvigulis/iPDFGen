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
    private const int IterationsPerThread = 10;
    private static readonly int Iterations = PdfGenDefaults.MaxDegreeOfParallelism * IterationsPerThread;

    [GlobalSetup]
    public async ValueTask Setup()
    {
        _puppeteerGenerator = new PuppeteerGenerator();
        await _puppeteerGenerator.Setup();

        _playwrightGenerator = new PlaywrightGenerator();
        await _playwrightGenerator.Setup();
    }

    [GlobalCleanup]
    public async ValueTask Cleanup()
    {
        await _puppeteerGenerator.DisposeAsync();
        await _playwrightGenerator.DisposeAsync();
    }

    [Benchmark]
    public async ValueTask PuppeteerSingle()
    {
        await using var stream = await _puppeteerGenerator.Generate();
    }

    [Benchmark]
    public async ValueTask PuppeteerMany()
    {
        await Parallel.ForEachAsync(new int[Iterations],
            new ParallelOptions
            {
                MaxDegreeOfParallelism = PdfGenDefaults.MaxDegreeOfParallelism
            },
            async (_, _) => await PuppeteerSingle());
    }

    [Benchmark]
    public async ValueTask PuppeteerSingleByUrl()
    {
        await using var stream = await _puppeteerGenerator.GenerateByUrl();
    }

    [Benchmark]
    public async ValueTask PlaywrightSingle()
    {
        await using var stream = await _playwrightGenerator.Generate();
    }

    [Benchmark]
    public async ValueTask PlaywrightMany()
    {
        await Parallel.ForEachAsync(new int[Iterations],
            new ParallelOptions
            {
                MaxDegreeOfParallelism = PdfGenDefaults.MaxDegreeOfParallelism
            },
            async (_, _) => await PlaywrightSingle());
    }

    [Benchmark]
    public async ValueTask PlaywrightSingleByUrl()
    {
        await using var stream = await _playwrightGenerator.GenerateByUrl();
    }
}