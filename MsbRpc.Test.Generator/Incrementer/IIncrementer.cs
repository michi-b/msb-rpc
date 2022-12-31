using MsbRpc.Generator.Attributes;
// ReSharper disable CheckNamespace
namespace Incrementer;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
    public void Store(int value);
    public void IncrementStored();
    public int GetStored();
}