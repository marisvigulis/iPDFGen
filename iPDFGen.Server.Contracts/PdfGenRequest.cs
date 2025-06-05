using Ardalis.GuardClauses;
using iPDFGen.Core.Models;

namespace iPDFGen.Server.Contracts;

public class PdfGenRequest
{
    public string? Body { get; set; }
    public string? Url { get; set; }
    public PdfGeneratorSettings? Settings { get; set; }
    public PdfGenProvider? Provider { get; set; }

    public bool IsGenerationByHtml => Body != null;

    public static PdfGenRequest FromUrl(string? url,
        PdfGeneratorSettings? settings = null,
        PdfGenProvider? provider = null
    )
    {
        Guard.Against.Null(url);
        return new PdfGenRequest
        {
            Url = url,
            Settings = settings,
            Provider = provider
        };
    }

    public static PdfGenRequest FromHtml(string? htmlBody,
        PdfGeneratorSettings? settings = null,
        PdfGenProvider? provider = null
    )
    {
        Guard.Against.Null(htmlBody);
        return new PdfGenRequest
        {
            Body = htmlBody,
            Settings = settings,
            Provider = provider
        };
    }
}