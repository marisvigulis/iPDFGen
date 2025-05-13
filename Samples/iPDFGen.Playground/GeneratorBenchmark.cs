using BenchmarkDotNet.Attributes;

namespace iPDFGen.Playground;

[ThreadingDiagnoser]
[MemoryDiagnoser]
public class GeneratorBenchmark
{
    private TestGenerator _generator = null!;

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
    public async ValueTask Generate()
    {
        await using var stream = await _generator.Generate();
        using var streamReader = new StreamReader(stream);
        await streamReader.ReadToEndAsync();
    }

    [Benchmark]
    public async ValueTask GenerateByUrl()
    {
        await using var stream = await _generator.GenerateByUrl();
        using var streamReader = new StreamReader(stream);
        await streamReader.ReadToEndAsync();
    }
}