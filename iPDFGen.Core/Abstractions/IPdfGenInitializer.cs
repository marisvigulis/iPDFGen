namespace iPDFGen.Core.Abstractions;

public interface IPdfGenInitializer
{
    ValueTask InitializeAsync();
}