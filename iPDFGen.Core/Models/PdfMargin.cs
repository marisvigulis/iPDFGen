namespace iPDFGen.Core.Models;

public record PdfMargin
{
    public PdfUnitOfMeasure? Top { get; set; }
    public PdfUnitOfMeasure? Bottom { get; set; }
    public PdfUnitOfMeasure? Left { get; set; }
    public PdfUnitOfMeasure? Right { get; set; }
}