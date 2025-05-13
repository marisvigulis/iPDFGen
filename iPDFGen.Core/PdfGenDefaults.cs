namespace iPDFGen.Core;

public static class PdfGenDefaults
{
    public const int DefaultTimeout = 60000;
    public static readonly int MaxDegreeOfParallelism = Environment.ProcessorCount * 2;
}