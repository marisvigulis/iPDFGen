using iPDFGen.Core.Models;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace iPDFGen.Puppeteer.Extensions;

internal static class PdfGenSettingsExtensions
{
    internal static PdfOptions ToPuppeteerPdfOptions(this PdfGenSettings? settings)
    {
        if (settings is null)
        {
            return new PdfOptions();
        }

        return new PdfOptions
        {
            DisplayHeaderFooter = settings.DisplayHeaderFooter,
            FooterTemplate = settings.FooterTemplate,
            Format = settings.Format.ToPuppeteerPaperFormat(),
            HeaderTemplate = settings.HeaderTemplate,
            PageRanges = settings.PageRange != null
                ? $"{settings.PageRange.From}-{settings.PageRange.To}"
                : null,
            MarginOptions = settings.Margin?.ToPuppeteerMarginOptions(),
            Scale = settings.Scale,
            Height = settings.Height,
            Width = settings.Width
        };
    }

    private static MarginOptions ToPuppeteerMarginOptions(this PdfMargin margin) =>
        new()
        {
            Bottom = margin.Bottom?.ToString(),
            Top = margin.Top?.ToString(),
            Left = margin.Left?.ToString(),
            Right = margin.Right?.ToString()
        };

    private static PaperFormat ToPuppeteerPaperFormat(this PdfFormat format)
    {
        return format switch
        {
            PdfFormat.Letter => PaperFormat.Letter,
            PdfFormat.Legal => PaperFormat.Legal,
            PdfFormat.Tabloid => PaperFormat.Tabloid,
            PdfFormat.Ledger => PaperFormat.Ledger,
            PdfFormat.A0 => PaperFormat.A0,
            PdfFormat.A1 => PaperFormat.A1,
            PdfFormat.A2 => PaperFormat.A2,
            PdfFormat.A3 => PaperFormat.A3,
            PdfFormat.A4 => PaperFormat.A4,
            PdfFormat.A5 => PaperFormat.A5,
            PdfFormat.A6 => PaperFormat.A6,
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Value not supported")
        };
    }
}