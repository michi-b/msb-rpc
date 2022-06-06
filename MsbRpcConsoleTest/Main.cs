using MsbRpc.GeneratorAttributes;

Console.WriteLine("Hello World");

[RpcInterface]
public interface IExampleRpcReceiver
{
    void SendInt(int value);
}