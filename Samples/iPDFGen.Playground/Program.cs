using BenchmarkDotNet.Running;
using iPDFGen.Playground;

BenchmarkRunner.Run<GeneratorBenchmark>();

// var remoteServer = new RemoteServerGenerator();
// await remoteServer.Setup();
// await remoteServer.Generate();