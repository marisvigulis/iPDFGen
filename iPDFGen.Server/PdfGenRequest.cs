using iPDFGen.Core.Models;

internal class PdfGenRequest
{
    public required string Body { get; set; }
    public PdfGeneratorSettings? Settings { get; set; }
}