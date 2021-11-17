using System;
using System.IO;
using System.IO.Compression;
#if __XAMARIN_ANDROID_v1_0__
using BrotliSharpLib;
#endif

namespace Titanium.Web.Proxy.Compression
{
    /// <summary>
    ///     A factory to generate the compression methods based on the type of compression
    /// </summary>
    internal static class CompressionFactory
    {
        internal static Stream Create(HttpCompression type, Stream stream, bool leaveOpen = true)
        {
            switch (type)
            {
                case HttpCompression.Gzip:
                    return new GZipStream(stream, CompressionMode.Compress, leaveOpen);
                case HttpCompression.Deflate:
                    return new DeflateStream(stream, CompressionMode.Compress, leaveOpen);
                case HttpCompression.Brotli:
                    return new BrotliStream(stream, CompressionMode.Compress, leaveOpen);
                default:
                    throw new Exception($"Unsupported compression mode: {type}");
            }
        }
    }
}
