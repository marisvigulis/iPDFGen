using System.Diagnostics;
using iPDFGen.Playground;

var generator = new TestGenerator();
await generator.Setup("resume.A4.html");

Directory.CreateDirectory("pdfs");
var timestamp = Stopwatch.GetTimestamp();
await Parallel.ForEachAsync(
    new int[100],
    new ParallelOptions
    {
        MaxDegreeOfParallelism = 32
    },
    async (i, _) =>
    {
        await using var streamResult = await generator.Generate();
        await using var stream = File.Create($"pdfs/resume.A4.{Guid.NewGuid()}.pdf");
        await streamResult.CopyToAsync(stream);
    });

Console.WriteLine($"Time taken: {Stopwatch.GetElapsedTime(timestamp)}");
Directory.Delete("pdfs", true);

// BenchmarkRunner.Run<GeneratorBenchmark>();