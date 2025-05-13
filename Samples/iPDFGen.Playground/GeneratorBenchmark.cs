using BenchmarkDotNet.Attributes;

namespace iPDFGen.Playground;

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
        var stream = await _generator.Generate();
        using var streamReader = new StreamReader(stream);
        await streamReader.ReadToEndAsync();

        await stream.DisposeAsync();
    }

    [Benchmark]
    public async ValueTask GenerateByUrl()
    {
        var stream = await _generator.GenerateByUrl();
        using var streamReader = new StreamReader(stream);
        await streamReader.ReadToEndAsync();

        await stream.DisposeAsync();
    }
}