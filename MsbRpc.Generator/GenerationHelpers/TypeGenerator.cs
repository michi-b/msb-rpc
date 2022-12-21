using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers;

public readonly struct TypeGenerator
{
    public readonly string Name;
    public SerializationKind SerializationKind { get; }

    public TypeGenerator(TypeInfo typeInfo)
    {
        string name = typeInfo.LocalName;
        string ns = typeInfo.Namespace;
        Name = $"{ns}.{name}";

        SerializationKind = SerializationTypeUtility.TryGetPrimitiveType(Name, out SerializationKind primitiveSerializationType)
            ? primitiveSerializationType
            : SerializationKind.Unresolved;
    }

    public bool TryGetConstantSizeInitializationLine(string variableName, out string result)
    {
        bool success = SerializationKind.TryGetConstantSizeCode(out string constantSizeCode);
        result = success ? $"const {Name} {variableName} = {constantSizeCode};" : string.Empty;
        return success;
    }

    public string GetBufferReadExpression(string responseReaderVariableName)
    {
        if (SerializationKind.GetIsPrimitive())
        {
            return $"{responseReaderVariableName}.{SerializationKind.GetBufferReadMethodName()}()";
        }

        throw new NotImplementedException();
    }
}