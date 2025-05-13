using iPDFGen.Core.Models;
using iPDFGen.Puppeteer.Extensions;

namespace iPDFGen.Puppeteer.Tests;

public class PdfGenSettingsToPuppeteerPdfOptionsTests
{
    [Theory]
    [InlineData(PdfFormat.Letter, 8.5, 11)]
    [InlineData(PdfFormat.Legal, 8.5, 14)]
    [InlineData(PdfFormat.Tabloid, 11, 17)]
    [InlineData(PdfFormat.Ledger, 17, 11)]
    [InlineData(PdfFormat.A0, 33.1, 46.8)]
    [InlineData(PdfFormat.A1, 23.4, 33.1)]
    [InlineData(PdfFormat.A2, 16.54, 23.4)]
    [InlineData(PdfFormat.A3, 11.7, 16.54)]
    [InlineData(PdfFormat.A4, 8.27, 11.7)]
    [InlineData(PdfFormat.A5, 5.83, 8.27)]
    [InlineData(PdfFormat.A6, 4.13, 5.83)]
    public void ToPuppeteerPaperFormat(PdfFormat pdfFormat, decimal expectedWidth, decimal expectedHeight)
    {
        var puppeteerPaperFormat = pdfFormat.ToPuppeteerPaperFormat();
        Assert.Equal(expectedWidth, puppeteerPaperFormat.Width);
        Assert.Equal(expectedHeight, puppeteerPaperFormat.Height);
    }


}