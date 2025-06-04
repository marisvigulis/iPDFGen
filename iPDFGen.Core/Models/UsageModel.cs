namespace iPDFGen.Core.Models;

public readonly struct UsageModel(
    int Available,
    long Used,
    int Max,
    long TotalProcessed,
    long TotalFailed,
    long TotalRequests
);