using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Extensions;
using iPDFGen.Playwright.Extensions;
using iPDFGen.Puppeteer.Extensions;
using iPDFGen.Server;
using iPDFGen.Server.Contracts;
using iPDFGen.Server.Middlewares;
using Microsoft.AspNetCore.Mvc;
using OneOf;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPdfGen(options =>
{
    options
        .SetMaxDegreeOfParallelism(EnvironmentVariables.LoadMaxDegreeOfParallelism())
        .SetDefaultTimeout(EnvironmentVariables.LoadDefaultTimeout());

    switch (EnvironmentVariables.LoadPdfGenProvider())
    {
        case PdfGenProvider.Playwright:
            options.UsePlaywright();
            break;
        case PdfGenProvider.Puppeteer:
            options.UsePuppeteer();
            break;
        default:
            throw new ArgumentOutOfRangeException();
    }
});

var app = builder.Build();
await app.Services.GetRequiredService<IPdfGenInitializer>().InitializeAsync();

if (args.Length == 0)
{
    app.UseMiddleware<RequestDecompressionMiddleware>();
    app.UseMiddleware<SharedSecretValidationMiddleware>();

    app.MapGet("api/alive", () => "I'm alive");

    app.MapGet("api/usage",
        async (HttpContext context) =>
        {
            return await context.RequestServices.GetRequiredService<IPdfGenerator>().UsageAsync();
        });

    app.MapPost("/api/pdf", async (HttpContext context, [FromBody] PdfGenRequest request) =>
    {
        var generator = context.RequestServices.GetRequiredService<IPdfGenerator>();

        var result = request.IsGenerationByHtml
            ? await generator.GenerateAsync(request.Body!, request.Settings)
            : await generator.GenerateByUrlAsync(request.Url!, request.Settings);

        return result.Match(
            success => Results.File(success.Stream, "application/pdf", "result.pdf"),
            err => Results.BadRequest(err)
        );
    });

    app.Run();
}