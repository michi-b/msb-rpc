namespace MsbRpc.Generator.Serialization;

public static class SerializationExtensions
{
    public static bool GetCanHandleRpcArguments(this ISerialization serialization) => serialization.GetIsResolved() && !serialization.GetIsVoid();

    public static bool GetCanHandleRpcResults(this ISerialization serialization) => serialization.GetIsResolved();
}