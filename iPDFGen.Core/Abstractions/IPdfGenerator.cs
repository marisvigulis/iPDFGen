using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Models;
using OneOf;

namespace iPDFGen.Core.Abstractions;

public interface IPdfGenerator
{
    public Task<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> Generate(string markup, PdfGenSettings? settings = null);
    public Task<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateByUrl(string url, PdfGenSettings? settings = null);
}