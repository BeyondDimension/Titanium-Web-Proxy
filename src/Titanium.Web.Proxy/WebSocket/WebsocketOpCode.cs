namespace Titanium.Web.Proxy.WebSocket;

public enum WebsocketOpCode : byte
{
    Continuation,
    Text,
    Binary,
    ConnectionClose = 8,
    Ping,
    Pong,
}
