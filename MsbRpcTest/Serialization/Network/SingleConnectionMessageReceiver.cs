using System.Net.Sockets;
using JetBrains.Annotations;
using MsbRpc.Messaging.Messenger;
using MsbRpc.Messaging.SocketOwners;

namespace MsbRpcTest.Serialization.Network;

public class SingleConnectionMessageReceiver : MessengerOwner
{
    public SingleConnectionMessageReceiver(Messenger messenger) : base(messenger) { }

    public readonly struct ListenResult
    {
        public ListenReturnCode ReturnCode { get; init; }
        public List<byte[]> Messages { get; init; }
    }

    public async Task<ListenResult> ListenAsync(CancellationToken cancellationToken, int timeout = 1000)
    {
        List<byte[]> messages = new();

        ReceiveMessageReturnCode lastReceiveMessageReturnCode;
        do
        {
            ReceiveMessageResult result = await Messenger.ReceiveMessageAsync();
            lastReceiveMessageReturnCode = result.MessageReturnCode;
            if (lastReceiveMessageReturnCode == ReceiveMessageReturnCode.Success)
            {
                messages.Add(result.Bytes);
            }
        } while (lastReceiveMessageReturnCode == ReceiveMessageReturnCode.Success);

        return new ListenResult
        {
            Messages = messages,
            ReturnCode = lastReceiveMessageReturnCode switch
            {
                ReceiveMessageReturnCode.Success => throw new InvalidOperationException(),
                ReceiveMessageReturnCode.ConnectionClosed => ListenReturnCode.ConnectionClosed,
                ReceiveMessageReturnCode.ConnectionClosedUnexpectedly => ListenReturnCode.ConnectionClosedUnexpectedly,
                _ => throw new ArgumentOutOfRangeException()
            }
        };
    }
}