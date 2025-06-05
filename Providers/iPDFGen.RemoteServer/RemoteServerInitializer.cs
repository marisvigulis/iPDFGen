using iPDFGen.Core;
using iPDFGen.Core.Abstractions;

namespace iPDFGen.RemoteServer;

public class RemoteServerInitializer : IPdfGenInitializer
{
    private readonly IGeneratorPool<bool> _generatorPool;

    public RemoteServerInitializer(IGeneratorPool<bool> generatorPool)
    {
        _generatorPool = generatorPool;
    }

    public ValueTask InitializeAsync()
    {
        return _generatorPool.InitializeAsync();
    }
}