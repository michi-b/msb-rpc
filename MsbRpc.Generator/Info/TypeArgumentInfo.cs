namespace MsbRpc.Generator.Info;

public struct TypeArgumentInfo
{
    public readonly string Name;
    public readonly string Type;

    public TypeArgumentInfo(string name, string type)
    {
        Name = name;
        Type = type;
    }
}