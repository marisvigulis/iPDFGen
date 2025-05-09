using iPDFGen.Core;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Models;
using OneOf;

namespace iPDFGen.Puppeteer;

public sealed class PuppeteerPdfGenerator: IPdfGenerator
{
    public Task<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> Generate(string markup, PdfGenSettings? settings = null)
    {
        throw new NotImplementedException();
    }

    public Task<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateByUrl(string url, PdfGenSettings? settings = null)
    {
        throw new NotImplementedException();
    }
}