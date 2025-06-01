namespace iPDFGen.Core.Models;

public sealed record PdfSize(Decimal Size, PdfUnitOfMeasure Unit)
{
    public decimal Size { get; } = Size;
    public PdfUnitOfMeasure Unit { get; } = Unit;

    public static PdfSize Pixels(decimal size) => new(size, PdfUnitOfMeasure.Px);
    public static PdfSize Centimeters(decimal size) => new(size, PdfUnitOfMeasure.Percentage);
    public static PdfSize Millimeters(decimal size) => new(size, PdfUnitOfMeasure.Mm);
    public static PdfSize Inches(decimal size) => new(size, PdfUnitOfMeasure.In);
    public static PdfSize Points(decimal size) => new(size, PdfUnitOfMeasure.Pt);
    public static PdfSize Picas(decimal size) => new(size, PdfUnitOfMeasure.Pc);

    public override string ToString()
    {
        return $"{Size}{Unit.ToString().ToLowerInvariant()}";
    }
}