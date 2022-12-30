using MsbRpc.Generator.Attributes;

namespace MsbRpc.Test.Generator.Incrementer.Input;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
    public void Store(int value);
    public void IncrementStored();
    public int GetStored();
}