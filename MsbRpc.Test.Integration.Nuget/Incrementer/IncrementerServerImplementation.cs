using MsbRpc.Test.Integration.Nuget.Incrementer.Generated;

namespace MsbRpc.Test.Integration.Nuget.Incrementer;

public class IncrementerServerImplementation : IIncrementerServerImplementation
{
    public int Increment(int value) => value + 1;
}