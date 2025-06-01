using iPDFGen.Core.Models;
using Microsoft.Playwright;

namespace iPDFGen.Playwright.Extensions;

public static class PdfGenSettingsExtensions
{
    public static PagePdfOptions ToPlaywrightPdfOptions(this PdfGenSettings? settings)
    {
        if (settings is null)
        {
            return new PagePdfOptions();
        }

        return new PagePdfOptions
        {
            DisplayHeaderFooter = settings.DisplayHeaderFooter,
            FooterTemplate = settings.FooterTemplate,
            Format = settings.Format.ToPuppeteerPaperFormat(),
            HeaderTemplate = settings.HeaderTemplate,
            PageRanges = settings.PageRange != null
                ? $"{settings.PageRange.From}-{settings.PageRange.To}"
                : null,
            Margin = settings.Margin?.ToPuppeteerMarginOptions(),
            Scale = (float)settings.Scale,
            Height = settings.Height?.ToString(),
            Width = settings.Width?.ToString(),
        };
    }

    private static Margin ToPuppeteerMarginOptions(this PdfMargin margin) =>
        new()
        {
            Bottom = margin.Bottom?.ToString(),
            Top = margin.Top?.ToString(),
            Left = margin.Left?.ToString(),
            Right = margin.Right?.ToString()
        };

    public static string ToPuppeteerPaperFormat(this PdfFormat format)
    {
        return format switch
        {
            PdfFormat.Letter => "Letter",
            PdfFormat.Legal => "Legal",
            PdfFormat.Tabloid => "Tabloid",
            PdfFormat.Ledger => "Ledger",
            PdfFormat.A0 => "A0",
            PdfFormat.A1 => "A1",
            PdfFormat.A2 => "A2",
            PdfFormat.A3 => "A3",
            PdfFormat.A4 => "A4",
            PdfFormat.A5 => "A5",
            PdfFormat.A6 => "A6",
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Value not supported")
        };
    }
}