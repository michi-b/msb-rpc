using MsbRpc.Test.Generator.Incrementer.Generated;

namespace MsbRpc.Test.Generator.Incrementer.Implementation;

internal class Incrementer : IIncrementerServerImplementation
{
    public int Increment(int value) => value + 1;
}