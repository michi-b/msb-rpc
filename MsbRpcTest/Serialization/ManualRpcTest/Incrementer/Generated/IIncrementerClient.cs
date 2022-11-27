namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.Generated;

public interface IIncrementerClient
{
    Task<int> Increment(int value);
}