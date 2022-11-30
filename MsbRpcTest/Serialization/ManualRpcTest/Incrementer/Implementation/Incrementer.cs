using MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Generated;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Implementation;

internal class Incrementer : IIncrementerServer
{
    public int Increment(int value) => value + 1;
}