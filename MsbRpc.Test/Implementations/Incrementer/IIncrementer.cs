using MsbRpc.Contracts;

// ReSharper disable CheckNamespace
namespace Incrementer;

public interface IIncrementer : IRpcContract
{
    int Increment(int value);
    public string? IncrementString(string? value);
    public void Store(int value);
    public void IncrementStored();
    public int GetStored();
    public void Finish();
}