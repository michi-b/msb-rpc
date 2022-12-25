namespace MsbRpc.Generator.HelperTree.Names;

public readonly struct EndPointNames
{
    public EndPointNames(ContractNames contract, EndPointTypeId endPointType)
    {
        LowerCaseTypeId = endPointType.GetLowerCaseName();
        UpperCaseTypeId = endPointType.GetUpperCaseName();
        
        UpperCaseBase = $"{contract.UpperCaseName}{UpperCaseTypeId}";
        LowerCaseBase = $"{contract.LowerCaseName}{UpperCaseTypeId}";
        
        EndPointType = $"{UpperCaseBase}Endpoint";
    }

    public string EndPointType { get; }
    
    public string LowerCaseTypeId { get; }

    public string UpperCaseTypeId { get; }
    
    /// <summary>{Contract}{Client/Server}</summary>
    public string UpperCaseBase { get; }
    
    /// <summary>{contract}{Client/Server}</summary>
    public string LowerCaseBase { get; }
    
    public string InterfaceType => $"I{UpperCaseBase}";
}