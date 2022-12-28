using MsbRpc.Generator.Attributes;

namespace MsbRpc.Test.Integration.Nuget.Incrementer;

[RpcContract]
public interface IIncrementer
{
    int Increment(int value);
}