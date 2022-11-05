using System.Diagnostics;
using System.Net.Sockets;
using MsbRpc.Messaging.Listeners;
using MsbRpc.Serialization;
using MsbRpc.Serialization.ByteArraySegment;

namespace MsbRpc.EndPoints;

public abstract class RpcServerEndPoint : RpcEndPoint
{
    private readonly object _executionStageLock = new();

    private readonly AutoResetEvent _receiveStageLock = new(true);
    private readonly object _sendResultStageLock = new();

    protected RpcServerEndPoint(Socket connectedSocket) : base(connectedSocket) { }

    public Task<Listener.ReturnCode> Listen(CancellationToken cancellationToken)
    {
        var listener = new ActiveListener(Messenger, ReceiveMessage, _receiveStageLock);
        return listener.Listen(cancellationToken);
    }

    private void ReceiveMessage(ArraySegment<byte> message)
    {
        SequentialReader messageReader = new(message);
        int procedure = messageReader.ReadInt32();
        ReceiveProcedureCall(procedure, ref messageReader);
    }

    protected void EnterExecutionStage()
    {
        //assert receive stage is locked
        Debug.Assert(!_receiveStageLock.WaitOne(0));

        //lock next state, then free up current
        Monitor.Enter(_executionStageLock);
        _receiveStageLock.Set();
    }

    protected void EnterSendResultStage()
    {
        //assert execution stage is locked
        Debug.Assert(Monitor.IsEntered(_executionStageLock));

        //lock next state, then free up current
        Monitor.Enter(_sendResultStageLock);
        Monitor.Exit(_executionStageLock);
    }

    protected void ExitSendResultStage()
    {
        //assert send result stage is locked
        Debug.Assert(Monitor.IsEntered(_sendResultStageLock));

        //free up current state
        Monitor.Exit(_sendResultStageLock);
    }

    /// <summary>
    ///     Called when a procedure call is received and to be dealt with.
    /// </summary>
    /// <remarks>
    ///     Make sure this method does not return until the server is ready to receive the next call.
    ///     If there is a long running task, make sure to finish utilizing the message reader before returning,
    ///     as it's bytes may afterwards be recycled by the listener.
    /// </remarks>
    protected abstract void ReceiveProcedureCall(int procedure, ref SequentialReader messageReader);
}