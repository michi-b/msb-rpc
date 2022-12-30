using MsbRpc.Test.Integration.Incrementer.Generated;

namespace MsbRpc.Test.Integration.Incrementer;

public class Incrementer : IIncrementerServerImplementation
{
    private int _value;

    public int Increment(int value) => value + 1;

    public void Store(int value)
    {
        _value = value;
    }

    public void IncrementStored()
    {
        _value++;
    }

    public int GetStored() => _value;
}