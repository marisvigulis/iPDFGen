using System.IO.Compression;

namespace iPDFGen.Server.Middlewares;

public class GzipRequestMiddleware
{
    private readonly RequestDelegate _next;

    public GzipRequestMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.ContentEncoding.Contains("gzip"))
        {
            var gzipStream = new GZipStream(context.Request.Body, CompressionMode.Decompress);
            context.Request.Body = gzipStream;
        }
        await _next(context);
    }
}

// Extension method to register the middleware
public static class GzipRequestMiddlewareExtensions
{
    public static IApplicationBuilder UseGzipRequestDecompression(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GzipRequestMiddleware>();
    }
}