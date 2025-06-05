using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Models;
using OneOf;

namespace iPDFGen.Core.Abstractions;

public interface IPdfGenerator
{
    ValueTask<UsageModel> UsageAsync();
    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> Generate(string markup, PdfGeneratorSettings? settings = null);
    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateByUrl(string url, PdfGeneratorSettings? settings = null);
}