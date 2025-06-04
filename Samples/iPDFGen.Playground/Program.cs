using BenchmarkDotNet.Running;
using iPDFGen.Playground;

BenchmarkRunner.Run<GeneratorBenchmark>();

// var playwright = new PlaywrightGenerator();
// await playwright.Setup("resume.A4.html");
// var a = playwright.Generate();