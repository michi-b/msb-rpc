namespace MsbRpcTest.Serialization.ManualRpcTest.Incrementer.ManualImplementation.Generated;

public interface IIncrementerProxy
{
    Task<int> Increment(int value);
}