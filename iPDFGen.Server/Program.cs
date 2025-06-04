using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Extensions;
using iPDFGen.Playwright.Extensions;
using iPDFGen.Puppeteer.Extensions;
using iPDFGen.Server;
using iPDFGen.Server.Middlewares;
using iPDFGen.Server.Models;
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
await app.Services.GetRequiredService<IPdfGenInitializer>().Initialize();

if (!args.Any())
{
    // app.UseHttpsRedirection();
    app.UseGzipRequestDecompression();

    app.MapGet("api/alive", () => "I'm alive");

    app.MapPost("/api/pdf", async (HttpContext context, [FromBody] PdfGenRequest request) =>
    {
        var generator = context.RequestServices.GetRequiredService<IPdfGenerator>();
        OneOf<PdfGenSuccessResult, PdfGenErrorResult> result = await generator.Generate(request.Body, request.Settings);

        return result.Match(
            success => Results.File(success.Stream, "application/pdf", "result.pdf"),
            err => Results.BadRequest(err)
        );
    });

    app.Run();
}