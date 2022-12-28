using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.GenerationTree.Names;

public readonly struct EndPointNames
{
    public EndPointNames(ContractNames contract, EndPointTypeId endPointType)
    {
        string upperCaseTypeId = endPointType.GetName();

        PascalCaseName = $"{contract.PascalCaseName}{upperCaseTypeId}";
        CamelCaseName = $"{contract.CamelCaseName}{upperCaseTypeId}";
        ImplementationInterface = $"{InterfacePrefix}{PascalCaseName}{ImplementationPostfix}";
    }

    /// <summary>{Contract}{Client/Server}</summary>
    public readonly string PascalCaseName;

    /// <summary>{contract}{Client/Server}</summary>
    public readonly string CamelCaseName;

    public readonly string ImplementationInterface;

    public const string DefaultBufferSizeConstant = "DefaultBufferSize";
}