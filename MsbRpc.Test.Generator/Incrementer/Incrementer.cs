// ReSharper disable once CheckNamespace
namespace Incrementer;

internal class Incrementer : IIncrementer
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