using MsbRpc.GeneratorAttributes;

namespace MsbRpcConsoleTest;

[RpcInterface]
public interface IExampleRpcReceiver
{
    void SendInt(int value);
}