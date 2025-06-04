using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Extensions;
using iPDFGen.Playwright.Extensions;
using iPDFGen.Puppeteer.Extensions;
using iPDFGen.Server;
using iPDFGen.Server.Middlewares;
using iPDFGen.Server.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPdfGen(s =>
{
    s
        .SetMaxDegreeOfParallelism(EnvironmentVariables.LoadMaxDegreeOfParallelism())
        .SetDefaultTimeout(EnvironmentVariables.LoadDefaultTimeout());

    switch (EnvironmentVariables.LoadPdfGenProvider())
    {
        case PdfGenProvider.Playwright:
            s.UsePlaywright();
            break;
        case PdfGenProvider.Puppeteer:
            s.UsePuppeteer();
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

    app.MapPost("/api/files/pdf", async (HttpContext context, [FromBody] PdfGenRequest request) =>
    {
        var generator = context.RequestServices.GetRequiredService<IPdfGenerator>();
        var result = await generator.Generate(request.Body, request.Settings);

        return result.Match(
            success => Results.File(success.Stream, "application/pdf", "result.pdf"),
            err => Results.BadRequest(err)
        );
    });

    app.Run();
}