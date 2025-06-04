using iPDFGen.Core.Models;

namespace iPDFGen.Server.Models;

internal class PdfGenRequest
{
    public required string Body { get; set; }
    public PdfGeneratorSettings? Settings { get; set; }
}