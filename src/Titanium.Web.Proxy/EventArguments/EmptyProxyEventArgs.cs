using Titanium.Web.Proxy.Network.TcpConnection;

namespace Titanium.Web.Proxy.EventArguments;

public class EmptyProxyEventArgs : ProxyEventArgsBase
{
    internal EmptyProxyEventArgs(ProxyServer server, TcpClientConnection clientConnection) : base(server, clientConnection)
    {
    }
}
