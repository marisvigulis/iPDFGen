using iPDFGen.Server.Contracts;

namespace iPDFGen.Server.Middlewares;

public class SharedSecretValidationMiddleware
{
    private readonly RequestDelegate _next;
    private static string _sharedSecret = EnvironmentVariables.LoadSharedSecret();

    public SharedSecretValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(PdfGenServerConstants.SharedSecretHeaderKey, out var sharedSecret) || !_sharedSecret.Equals(sharedSecret, StringComparison.Ordinal))
        {
            context.Response.StatusCode = 401;
            return;
        }

        await _next(context);
    }
}