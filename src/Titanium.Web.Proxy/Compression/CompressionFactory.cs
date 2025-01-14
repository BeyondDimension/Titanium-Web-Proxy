﻿using System.IO.Compression;
using Titanium.Web.Proxy.Models;

namespace Titanium.Web.Proxy.Compression;

/// <summary>
///     A factory to generate the compression methods based on the type of compression
/// </summary>
internal static class CompressionFactory
{
    internal static Stream Create(HttpCompression type, Stream stream, bool leaveOpen = true)
    {
        return type switch
        {
            HttpCompression.Gzip => new GZipStream(stream, CompressionMode.Compress, leaveOpen),
            HttpCompression.Deflate => new DeflateStream(stream, CompressionMode.Compress, leaveOpen),
            HttpCompression.Brotli => new BrotliStream(stream, CompressionMode.Compress, leaveOpen),
            _ => throw new Exception($"Unsupported compression mode: {type}"),
        };
    }
}
