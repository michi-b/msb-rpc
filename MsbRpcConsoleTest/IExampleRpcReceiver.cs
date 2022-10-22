using MsbRpc.GeneratorAttributes;

namespace MsbRpcConsoleTest;

[RpcContract]
public interface IExampleRpcReceiver
{
    void SendInt(int value);
}