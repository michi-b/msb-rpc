using MsbRpcTest.ManualRpcTest.Incrementer.Generated;

namespace MsbRpcTest.ManualRpcTest.Incrementer.Implementation;

internal class Incrementer : IIncrementerServerImplementation
{
    public int Increment(int value) => value + 1;
}