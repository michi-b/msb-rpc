namespace MsbRpc.Generator.GenerationTree.Names;

public readonly struct EndPointNames
{
    public EndPointNames(ContractNames contract, EndPointTypeId endPointType)
    {
        string upperCaseTypeId = endPointType.GetName();

        PascalCaseName = $"{contract.PascalCaseName}{upperCaseTypeId}";
        CamelCaseName = $"{contract.CamelCaseName}{upperCaseTypeId}";

        InterfaceType = $"I{PascalCaseName}";
    }

    /// <summary>{Contract}{Client/Server}</summary>
    public readonly string PascalCaseName;

    /// <summary>{contract}{Client/Server}</summary>
    public readonly string CamelCaseName;

    public readonly string InterfaceType;
    public const string DefaultBufferSizeConstant = "DefaultBufferSize";
}