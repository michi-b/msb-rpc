﻿using System.Net.Sockets;
using MsbRpc.EndPoints;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Primitives;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Implementation.Generated;

public class IncrementerServerEndPoint : RpcServerEndPoint
{
    private readonly IIncrementerServer _incrementerServerImplementation;

    protected IncrementerServerEndPoint(Socket connectedSocket, IIncrementerServer incrementerServerImplementation)
        : base(connectedSocket)
        => _incrementerServerImplementation = incrementerServerImplementation;

    protected override void ReceiveProcedureCall(int procedure, ref ByteArraySegmentReader messageReader)
    {
        switch ((IncrementerProcedure)procedure)
        {
            case IncrementerProcedure.Increment:
                ScheduleIncrement(ref messageReader);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ScheduleIncrement(ref ByteArraySegmentReader messageReader)
    {
        //todo: keep track of current stage and handle exceptions properly
        
        int value = messageReader.ReadInt32();
        Task.Run(() => ExecuteIncrement(value));
    }

    private Task ExecuteIncrement(int value)
    {
        //todo: keep track of current stage and handle exceptions properly
        
        EnterExecutionStage();

        int result = _incrementerServerImplementation.Increment(value);

        EnterSendResultStage();

        ArraySegment<byte> resultBuffer = BorrowBuffer(PrimitiveSerializer.Int32Size);
        resultBuffer.WriteInt32(result);
        Messenger.SendMessage(ref resultBuffer);

        ExitSendResultStage();

        return Task.CompletedTask;
    }
}