using MsbRpc.Generator.Attributes;

namespace MsbRpc.Test.Integration.Incrementer;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
    public void Store(int value);
    public void IncrementStored();
    public int GetStored();
}