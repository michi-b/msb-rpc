using MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Input;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Implementation;

internal class Incrementer : IIncrementer
{
    public int Increment(int value) => value + 1;
}