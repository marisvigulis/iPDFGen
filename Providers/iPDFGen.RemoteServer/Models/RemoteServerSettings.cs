namespace iPDFGen.RemoteServer.Models;

public class RemoteServerSettings
{
    /// <summary>
    /// Base url for Remote server
    /// </summary>
    public required string BaseUrl { get; init; }

    /// <summary>
    /// SharedSecret for Remote server to ensure you can access it
    /// </summary>
    public required string SharedSecret { get; init; }

    /// <summary>
    /// Compression method:
    /// None, Gzip, Deflate
    /// </summary>
    public required CompressionType CompressionType { get; init; }
}