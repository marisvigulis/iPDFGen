namespace iPDFGen.Core.Models;

public sealed class PdfGeneratorSettings
{
    /// <summary>
    /// Scales the rendering of the web page. Amount must be between 0.1 and 2
    /// Default is 1.
    /// </summary>
    public decimal Scale { get; init; } = 1;

    /// <summary>
    /// Resulting PDF format. Default is A4.
    /// </summary>
    public PdfFormat Format { get; init; } = PdfFormat.A4;

    /// <summary>
    /// Timeout in milliseconds. Default is 30s.
    /// </summary>
    public int? Timeout { get; init; }

    /// <summary>
    /// Shall display header and footer
    /// </summary>
    public bool DisplayHeaderFooter { get; init; } = false;

    /// <summary>
    /// HTML template for the print header. Should be valid HTML with the following
    /// classes used to inject values into them:
    /// - `date` formatted print date
    /// - `title` document title
    /// - `url` document location
    /// - `pageNumber` current page number
    /// - `totalPages` total pages in the document
    /// </summary>
    public string? HeaderTemplate { get; init; }

    /// <summary>
    /// HTML template for the footer. Should be valid HTML with the following
    /// classes used to inject values into them:
    /// - `date` formatted print date
    /// - `title` document title
    /// - `url` document location
    /// - `pageNumber` current page number
    /// - `totalPages` total pages in the document
    /// </summary>
    public string? FooterTemplate { get; init; }

    /// <summary>
    /// Pages range that shall be included into the resulting PDF
    /// </summary>
    public PdfPageRange? PageRange { get; init; }

    /// <summary>
    /// Width of a page
    /// </summary>
    public PdfSize? Width { get; init; }

    /// <summary>
    /// Height of a page
    /// </summary>
    public PdfSize? Height { get; init; }

    /// <summary>
    /// Predefined page margins
    /// </summary>
    public PdfMargin? Margin { get; init; }
}