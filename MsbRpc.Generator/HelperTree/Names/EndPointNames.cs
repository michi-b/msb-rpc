namespace MsbRpc.Generator.HelperTree.Names;

public readonly struct EndPointNames
{
    public EndPointNames(ContractNames contract, EndPointTypeId endPointType)
    {
        LowerCaseTypeId = endPointType.GetLowerCaseName();
        UpperCaseTypeId = endPointType.GetUpperCaseName();
        
        PascalCaseName = $"{contract.PascalCaseName}{UpperCaseTypeId}";
        CamelCaseName = $"{contract.CamelCaseName}{UpperCaseTypeId}";
        
        EndPointType = $"{PascalCaseName}Endpoint";
        InterfaceType = $"I{PascalCaseName}";
    }

    public readonly string EndPointType;
    public readonly string LowerCaseTypeId;
    public readonly string UpperCaseTypeId;

    /// <summary>{Contract}{Client/Server}</summary>
    public readonly string PascalCaseName;
    /// <summary>{contract}{Client/Server}</summary>
    public readonly string CamelCaseName;
    public readonly string InterfaceType;
    public const string DefaultBufferSizeConstant = "DefaultBufferSize";
}