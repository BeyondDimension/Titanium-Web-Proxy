﻿using System;
using System.IO;
using System.IO.Compression;
#if __XAMARIN_ANDROID_v1_0__
using BrotliSharpLib;
#endif

namespace Titanium.Web.Proxy.Compression
{
    /// <summary>
    ///     A factory to generate the de-compression methods based on the type of compression
    /// </summary>
    internal class DecompressionFactory
    {
        internal static Stream Create(HttpCompression type, Stream stream, bool leaveOpen = true)
        {
            switch (type)
            {
                case HttpCompression.Gzip:
                    return new GZipStream(stream, CompressionMode.Decompress, leaveOpen);
                case HttpCompression.Deflate:
                    return new DeflateStream(stream, CompressionMode.Decompress, leaveOpen);
                case HttpCompression.Brotli:
                    return new BrotliStream(stream, CompressionMode.Decompress, leaveOpen);
                default:
                    throw new Exception($"Unsupported decompression mode: {type}");
            }
        }
    }
}
