namespace Titanium.Web.Proxy.Network.Writers;

internal class NullWriter : IHttpStreamWriter
{
    public static NullWriter Instance { get; } = new NullWriter();

    public bool IsNetworkStream => false;

    private NullWriter()
    {
    }

    public void Write(byte[] buffer, int offset, int count)
    {
    }

    public Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
#if NET451
        return Net45Compatibility.CompletedTask;
#else
        return Task.CompletedTask;
#endif
    }

    public ValueTask WriteLineAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask WriteLineAsync(string value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
