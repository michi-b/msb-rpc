using Incrementer;

namespace MsbRpc.Test.Generator.Incrementer;

internal class Incrementer : IIncrementer
{
    private int _value;
    public bool RanToCompletion { get; private set; }

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

    public void Finish()
    {
        RanToCompletion = true;
    }

    public string IncrementString(string value) => (int.Parse(value) + 1).ToString();

    public static Incrementer Create() => new();

    public void Dispose() { }
}