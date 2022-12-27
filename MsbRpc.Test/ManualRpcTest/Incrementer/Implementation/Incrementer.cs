using MsbRpc.Test.ManualRpcTest.Incrementer.Generated;

namespace MsbRpc.Test.ManualRpcTest.Incrementer.Implementation;

internal class Incrementer : IIncrementerServerImplementation
{
    public int Increment(int value) => value + 1;
}