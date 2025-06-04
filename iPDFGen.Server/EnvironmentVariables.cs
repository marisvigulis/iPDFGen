using iPDFGen.Core;

namespace iPDFGen.Server;

public static class EnvironmentVariables
{
    public static int LoadMaxDegreeOfParallelism()
    {
        var maxDegreeOfParallelismStr = Environment.GetEnvironmentVariable("MAX_DEGREE_OF_PARALELLISM");

        if (maxDegreeOfParallelismStr is not null && int.TryParse(maxDegreeOfParallelismStr, out var maxDegreeOfParallelism))
        {
            return maxDegreeOfParallelism;
        }

        return PdfGenDefaults.MaxDegreeOfParallelism;
    }

    public static TimeSpan LoadDefaultTimeout()
    {
        var maxDegreeOfParallelismStr = Environment.GetEnvironmentVariable("DEFAULT_TIMEOUT");

        if (maxDegreeOfParallelismStr is not null && double.TryParse(maxDegreeOfParallelismStr, out var timeoutInSeconds))
        {
            return TimeSpan.FromSeconds(timeoutInSeconds);
        }

        return PdfGenDefaults.DefaultTimeout;
    }


    public static PdfGenProvider LoadPdfGenProvider()
    {
        var maxDegreeOfParallelismStr = Environment.GetEnvironmentVariable("PDFGEN_PROVIDER");

        if (maxDegreeOfParallelismStr is not null && Enum.TryParse<PdfGenProvider>(maxDegreeOfParallelismStr, out var pdfGenProvider))
        {
            return pdfGenProvider;
        }

        return PdfGenProvider.Playwright;
    }
}