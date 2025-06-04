namespace iPDFGen.Core.Models;

/// <summary>
/// Margin definition
/// </summary>
public record PdfMargin
{
    /// <summary>
    /// Top margin
    /// </summary>
    public PdfSize? Top { get; set; }
    /// <summary>
    /// Bottom margin
    /// </summary>
    public PdfSize? Bottom { get; set; }
    /// <summary>
    /// Left margin
    /// </summary>
    public PdfSize? Left { get; set; }
    /// <summary>
    /// Right margin
    /// </summary>
    public PdfSize? Right { get; set; }
}