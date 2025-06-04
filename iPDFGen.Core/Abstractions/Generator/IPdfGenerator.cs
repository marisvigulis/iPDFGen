using iPDFGen.Core.Models;
using OneOf;

namespace iPDFGen.Core.Abstractions.Generator;

public interface IPdfGenerator
{
    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> Generate(string markup, PdfGeneratorSettings? settings = null);
    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateByUrl(string url, PdfGeneratorSettings? settings = null);
}