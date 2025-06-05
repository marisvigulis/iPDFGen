using System.Net;
using System.Net.Mime;
using System.Text;
using Ardalis.GuardClauses;
using iPDFGen.Core;
using iPDFGen.Core.Abstractions;
using iPDFGen.Core.Abstractions.Generator;
using iPDFGen.Core.Models;
using iPDFGen.RemoteServer.Models;
using iPDFGen.Server.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneOf;

namespace iPDFGen.RemoteServer;

public class RemoteServerGenerator : IPdfGenerator
{
    private readonly IGeneratorPool<bool> _remoteServerGeneratorPool;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly RemoteServerSettings _serverSettings;
    private readonly Uri _generateUri;
    public RemoteServerGenerator(
        IGeneratorPool<bool> remoteServerGeneratorPool,
        IHttpClientFactory httpClientFactory,
        RemoteServerSettings serverSettings)
    {
        _remoteServerGeneratorPool = remoteServerGeneratorPool;
        _httpClientFactory = httpClientFactory;
        _serverSettings = serverSettings;
        _generateUri = new Uri(new Uri(_serverSettings.BaseUrl, UriKind.Absolute), new Uri("/api/pdf", UriKind.Relative));
    }

    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateAsync(string markup,
        PdfGeneratorSettings? settings = null)
    {
        return _remoteServerGeneratorPool.RunAsync(async (_, cancellationToken) =>
        {
            var client = _httpClientFactory.CreateClient("RemoteServer");
            var response =
                await client.PostAsync(_generateUri, ComposeRequest(markup, null, settings), cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return new PdfGenSuccessResult
                {
                    Stream = await response.Content.ReadAsStreamAsync(cancellationToken)
                };
            }

            var errorSerialized = await response.Content.ReadAsStringAsync(cancellationToken);
            return HandleError(errorSerialized, response.StatusCode);
        });
    }


    public ValueTask<OneOf<PdfGenSuccessResult, PdfGenErrorResult>> GenerateByUrlAsync(string url,
        PdfGeneratorSettings? settings = null)
    {
        return _remoteServerGeneratorPool.RunAsync(async (_, cancellationToken) =>
        {
            var client = _httpClientFactory.CreateClient("RemoteServer");
            var response = await client.PostAsync(_generateUri, ComposeRequest(null, url, settings), cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return new PdfGenSuccessResult
                {
                    Stream = await response.Content.ReadAsStreamAsync(cancellationToken)
                };
            }

            var errorSerialized = await response.Content.ReadAsStringAsync(cancellationToken);
            return HandleError(errorSerialized, response.StatusCode);
        });
    }

    private HttpContent ComposeRequest(string? markup, string? url, PdfGeneratorSettings? settings)
    {
        var genModel = markup is null
            ? PdfGenRequest.FromUrl(url, settings)
            : PdfGenRequest.FromHtml(markup, settings);

        var httpContent = new StringContent(JsonConvert.SerializeObject(genModel, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        }), Encoding.UTF8, MediaTypeNames.Application.Json);

        Guard.Against.Null(_serverSettings.SharedSecret);

        httpContent.Headers.Add(PdfGenServerConstants.SharedSecretHeaderKey, _serverSettings.SharedSecret);

        if (_serverSettings.CompressionType == CompressionType.None)
        {
            return httpContent;
        }

        return new CompressedContent(httpContent, _serverSettings.CompressionType.ToString());
    }

    private static PdfGenErrorResult HandleError(string errorSerialized, HttpStatusCode statusCode)
    {
        var errorResult = JsonConvert.DeserializeObject<PdfGenErrorResult>(errorSerialized);
        return errorResult ??
               new PdfGenErrorResult(
                   "0005",
                   $"Remote server responded with {statusCode} and can't deserialize, response: ${errorSerialized}"
               );
    }
}