namespace MsbRpc.Messaging;

public enum ListenReturnCode
{
    /// <summary>
    ///     listening stopped by connection close
    /// </summary>
    ConnectionClosed,

    /// <summary>
    ///     listening stopped because the receive delegate returned true
    /// </summary>
    OperationDiscontinued,
    ConnectionDisposed
}