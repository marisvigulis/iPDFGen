namespace iPDFGen.Core.Abstractions.Generator;

public readonly struct PdfGenErrorResult
{
    public required string Code { get; init; }
    public required string Message { get; init; }
}