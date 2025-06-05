using System.IO.Compression;

namespace iPDFGen.Server.Middlewares;

public class RequestDecompressionMiddleware
{
    private readonly RequestDelegate _next;

    public RequestDecompressionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.ContentEncoding.Contains("gzip", StringComparer.OrdinalIgnoreCase))
        {
            var gzipStream = new GZipStream(context.Request.Body, CompressionMode.Decompress);
            context.Request.Body = gzipStream;
        }
        if (context.Request.Headers.ContentEncoding.Contains("deflate", StringComparer.OrdinalIgnoreCase))
        {
            var deflateStream = new DeflateStream(context.Request.Body, CompressionMode.Decompress);
            context.Request.Body = deflateStream;
        }
        await _next(context);
    }
}