namespace iPDFGen.Core.Abstractions;

public interface IPdfGenInitializer
{
    Task Initialize();
    Task EnsureInitialized();
}