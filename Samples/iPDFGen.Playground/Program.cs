using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using iPDFGen.Playground;

Summary result = BenchmarkRunner.Run<GeneratorBenchmark>();