using iPDFGen.Core.Models;
using iPDFGen.Puppeteer.Extensions;

namespace iPDFGen.Puppeteer.Tests;

public class OptionsMappingTests
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
    
    [Theory]
    [InlineData(10.5, PdfUnitOfMeasure.Ch, "10.5ch")]
    [InlineData(20, PdfUnitOfMeasure.Cm, "20cm")]
    [InlineData(30.7, PdfUnitOfMeasure.Em, "30.7em")]
    [InlineData(40, PdfUnitOfMeasure.Ex, "40ex")]
    [InlineData(50.3, PdfUnitOfMeasure.In, "50.3in")]
    [InlineData(60, PdfUnitOfMeasure.Mm, "60mm")]
    [InlineData(70.8, PdfUnitOfMeasure.Pc, "70.8pc")]
    [InlineData(80, PdfUnitOfMeasure.Pt, "80pt")]
    [InlineData(90.2, PdfUnitOfMeasure.Px, "90.2px")]
    [InlineData(100, PdfUnitOfMeasure.Rem, "100rem")]
    public void PdfSizeToString_ReturnsCorrectFormat(decimal value, PdfUnitOfMeasure unit, string expected)
    {
        var size = new PdfSize(value, unit);
        Assert.Equal(expected, size.ToString());
    }
    
    [Fact]
    public void ToPuppeteerPdfOptions_MapsAllFieldsCorrectly()
    {
        var settings = new PdfGeneratorSettings
        {
            Format = PdfFormat.A4,
            Width = PdfSize.Pixels(100),
            Height = PdfSize.Centimeters(200),
            Scale = 1.5m,
            DisplayHeaderFooter = true,
            PageRange = new PdfPageRange(5, 6),
            FooterTemplate = "FooterTemplate",
            HeaderTemplate = "HeaderTemplate"
        };

        var options = settings.ToPuppeteerPdfOptions();

        Assert.Equal(settings.Width.ToString(), options.Width);
        Assert.Equal(settings.Height.ToString(), options.Height);
        Assert.Equal(settings.Scale, options.Scale);
        Assert.Equal(settings.DisplayHeaderFooter, options.DisplayHeaderFooter);
        Assert.Equal($"{settings.PageRange.From}-{settings.PageRange.To}", options.PageRanges);
        Assert.Equal(settings.FooterTemplate, options.FooterTemplate);
        Assert.Equal(settings.HeaderTemplate, options.HeaderTemplate);
    }

}