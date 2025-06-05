using System.IO.Compression;
using System.Net;

namespace iPDFGen.RemoteServer;

public class CompressedContent : HttpContent
{
    private readonly string _encodingType;
    private readonly HttpContent _originalContent;

    public CompressedContent(HttpContent content, string encodingType)
    {
        ArgumentNullException.ThrowIfNull(encodingType);

        _originalContent = content ?? throw new ArgumentNullException(nameof(content));
        _encodingType = encodingType.ToLowerInvariant();

        if (_encodingType != "gzip" && _encodingType != "deflate")
            throw new InvalidOperationException(
                $"Encoding '{_encodingType}' is not supported. Only supports gzip or deflate encoding.");

        foreach (var header in _originalContent.Headers) Headers.TryAddWithoutValidation(header.Key, header.Value);

        Headers.ContentEncoding.Add(encodingType);
    }


    protected override bool TryComputeLength(out long length)
    {
        length = -1;

        return false;
    }

    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
    {
        Stream compressedStream = _encodingType switch
        {
            "gzip" => new GZipStream(stream, CompressionMode.Compress, true),
            "deflate" => new DeflateStream(stream, CompressionMode.Compress, true),
            _ => throw new InvalidOperationException($"Encoding '{_encodingType}' is not supported.")
        };

        return _originalContent
            .CopyToAsync(compressedStream)
            .ContinueWith(_ => { compressedStream.Dispose(); });
    }
}