using MsbRpc.Test.Generator.Incrementer.Generated;

namespace MsbRpc.Test.Generator.Incrementer.Implementation;

internal class Incrementer : IIncrementerServerImplementation
{
    private int _stored;
    
    public int Increment(int value) => value + 1;
    
    public void Store(int value)
    {
        _stored = value;
    }

    public void IncrementStored()
    {
        _stored++;
    }

    public int GetStored() => _stored;
}