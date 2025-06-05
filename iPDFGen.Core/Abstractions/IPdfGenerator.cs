using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Models;
using OneOf;

namespace iPDFGen.Core.Abstractions;

public interface IPdfGenerator
{
    ValueTask<OneOf<UsageModel, PdfGenErrorResult>> UsageAsync();
    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateAsync(string markup, PdfGeneratorSettings? settings = null);
    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateByUrlAsync(string url, PdfGeneratorSettings? settings = null);
}