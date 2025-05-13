using BenchmarkDotNet.Attributes;
using iPDFGen.Core;

namespace iPDFGen.Playground;

[ThreadingDiagnoser]
[MemoryDiagnoser]
public class GeneratorBenchmark
{
    private TestGenerator _generator = null!;
    private const int IterationsPerThread = 2;

    [GlobalSetup]
    public async ValueTask Setup()
    {
        _generator = new TestGenerator();
        await _generator.Setup("resume.A4.html");
    }

    [GlobalCleanup]
    public ValueTask Cleanup()
    {
        return _generator.DisposeAsync();
    }

    [Benchmark]
    public async ValueTask GenerateSingle()
    {
        await using var stream = await _generator.Generate();
        using var streamReader = new StreamReader(stream);
        await streamReader.ReadToEndAsync();
    }

    [Benchmark]
    public async ValueTask GenerateMany()
    {
        await Parallel.ForEachAsync(new int[PdfGenDefaults.MaxDegreeOfParallelism * IterationsPerThread],
            new ParallelOptions
            {
                MaxDegreeOfParallelism = PdfGenDefaults.MaxDegreeOfParallelism
            },
            async (_, _) => await GenerateSingle());
    }
}