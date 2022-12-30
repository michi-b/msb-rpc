namespace MsbRpc.Test.Generator.Incrementer.Generated;

public interface IIncrementerServerImplementation
{
    int Increment(int value);
    void Store(int value);
    void IncrementStored();
    int GetStored();
}