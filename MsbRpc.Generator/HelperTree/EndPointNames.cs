namespace MsbRpc.Generator.HelperTree;

public readonly struct EndPointNames
{
    public EndPointNames(ContractNames contract, EndPointTypeId endPointType)
    {
        LowerCaseTypeId = endPointType.GetLowerCaseName();
        UpperCaseTypeId = endPointType.GetUpperCaseName();
        
        UpperCaseBase = $"{contract.UpperCaseName}{UpperCaseTypeId}";
        LowerCaseBase = $"{contract.LowerCaseName}{LowerCaseTypeId}";
        
        EndPointType = $"{UpperCaseBase}Endpoint";
    }

    public string EndPointType { get; }
    
    public string LowerCaseTypeId { get; }

    public string UpperCaseTypeId { get; }
    
    /// <summary>{Contract}{Client/Server}</summary>
    public string UpperCaseBase { get; }
    
    /// <summary>{contract}{Client/Server}</summary>
    public string LowerCaseBase { get; }
}