namespace iPDFGen.Core.Models;

public class PdfGenRegistrationSettings
{
    public TimeSpan Timeout { get; init; }
    public int MaxDegreeOfParallelism { get; init; }
}