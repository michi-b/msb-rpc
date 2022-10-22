namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Implementation.Generated;

public interface IIncrementerClient
{
    Task<int> Increment(int value);
}