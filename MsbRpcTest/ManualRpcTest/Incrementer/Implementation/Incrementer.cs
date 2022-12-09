using MsbRpcTest.ManualRpcTest.Incrementer.Generated;

namespace MsbRpcTest.ManualRpcTest.Incrementer.Implementation;

internal class Incrementer : IIncrementerServer
{
    public int Increment(int value) => value + 1;
}