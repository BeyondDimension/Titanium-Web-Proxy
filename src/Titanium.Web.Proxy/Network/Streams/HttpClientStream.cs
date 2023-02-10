using Titanium.Web.Proxy.Http;
using Titanium.Web.Proxy.Models;
using Titanium.Web.Proxy.Network.BufferPool;
using Titanium.Web.Proxy.Network.TcpConnection;

namespace Titanium.Web.Proxy.Network.Streams;

internal sealed class HttpClientStream : HttpStream
{
    public TcpClientConnection Connection { get; }

    internal HttpClientStream(ProxyServer server, TcpClientConnection connection, Stream stream, IBufferPool bufferPool, CancellationToken cancellationToken)
        : base(server, stream, bufferPool, cancellationToken)
    {
        Connection = connection;
    }

    /// <summary>
    ///     Writes the response.
    /// </summary>
    /// <param name="response">The response object.</param>
    /// <param name="cancellationToken">Optional cancellation token for this async task.</param>
    /// <returns>The Task.</returns>
    internal async ValueTask WriteResponseAsync(Response response, CancellationToken cancellationToken = default)
    {
        var headerBuilder = new HeaderBuilder();

        // Write back response status to client
        headerBuilder.WriteResponseLine(response.HttpVersion, response.StatusCode, response.StatusDescription);

        await WriteAsync(response, headerBuilder, cancellationToken);
    }

    internal async ValueTask<RequestStatusInfo> ReadRequestLine(CancellationToken cancellationToken = default)
    {
        // read the first line HTTP command
        var httpCmd = await ReadLineAsync(cancellationToken);
        if (string.IsNullOrEmpty(httpCmd))
        {
            return default;
        }

        Request.ParseRequestLine(httpCmd!, out var method, out var requestUri, out var version);

        return new RequestStatusInfo { Method = method, RequestUri = requestUri, Version = version };
    }
}
