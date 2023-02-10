﻿namespace Titanium.Web.Proxy.Network.Writers;

/// <summary>
///     A concrete implementation of this interface is required when calling CopyStream.
/// </summary>
public interface IHttpStreamWriter
{
    bool IsNetworkStream { get; }

    void Write(byte[] buffer, int offset, int count);

    Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

    ValueTask WriteLineAsync(CancellationToken cancellationToken = default);

    ValueTask WriteLineAsync(string value, CancellationToken cancellationToken = default);
}
