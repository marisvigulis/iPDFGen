namespace iPDFGen.Core.Models;

public class PdfGenSettings
{
    /// <summary>
    /// Scales the rendering of the web page. Amount must be between 0.1 and 2
    /// Default is 1.
    /// </summary>
    public decimal Scale { get; set; } = 1;

    /// <summary>
    /// Resulting PDF format. Default is A4.
    /// </summary>
    public PdfFormat Format { get; set; } = PdfFormat.A4;

    /// <summary>
    /// Timeout in milliseconds. Default is 60s.
    /// </summary>
    public int Timeout { get; set; } = PdfGenDefaults.DefaultTimeout;

    /// <summary>
    /// Shall display header and footer
    /// </summary>
    public bool DisplayHeaderFooter { get; set; } = false;

    /// <summary>
    /// HTML template for the print header. Should be valid HTML with the following
    /// classes used to inject values into them:
    /// - `date` formatted print date
    /// - `title` document title
    /// - `url` document location
    /// - `pageNumber` current page number
    /// - `totalPages` total pages in the document
    /// </summary>
    public string? HeaderTemplate { get; set; }

    public string? FooterTemplate { get; set; }
    public PdfPageRange? PageRange { get; set; }
    public string? Width { get; set; }
    public string? Height { get; set; }
    public PdfMargin? Margin { get; set; }
}