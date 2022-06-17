namespace MsbRpc.Messaging;

public enum ReceiveReturnCode
{
    Success = 0,
    ConnectionClosed = 1,
    ConnectionClosedUnexpectedly = 2
}