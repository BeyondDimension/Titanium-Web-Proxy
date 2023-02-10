using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Network.Streams;
using Titanium.Web.Proxy.Network.Writers;

namespace Titanium.Web.Proxy.Network.Readers;

public interface IHttpStreamReader : ILineStream
{
    int Read(byte[] buffer, int offset, int count);

    Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);

    Task CopyBodyAsync(IHttpStreamWriter writer, bool isChunked, long contentLength,
       bool isRequest, SessionEventArgs args, CancellationToken cancellationToken);
}
