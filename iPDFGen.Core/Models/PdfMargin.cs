namespace iPDFGen.Core.Models;

public record PdfMargin
{
    public PdfSize? Top { get; set; }
    public PdfSize? Bottom { get; set; }
    public PdfSize? Left { get; set; }
    public PdfSize? Right { get; set; }
}