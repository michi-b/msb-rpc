using MsbRpcTest.Serialization.ManualRpcTest.Incrementer.ManualImplementation.Input;

namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.ManualImplementation;

internal class Incrementer : IIncrementer
{
    public int Increment(int value) => value + 1;
}