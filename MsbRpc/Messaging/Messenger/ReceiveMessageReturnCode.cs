namespace MsbRpc.Messaging.Messenger;

public enum ReceiveMessageReturnCode
{
    Success = 0,
    ConnectionClosed = 1,
    ConnectionClosedUnexpectedly = 2
}