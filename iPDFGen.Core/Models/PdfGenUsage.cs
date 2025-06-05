namespace iPDFGen.Core.Models;

public record PdfGenUsage(
    int Available,
    int Used,
    int Max,
    long TotalProcessed,
    long TotalFailed,
    long TotalRequests
);