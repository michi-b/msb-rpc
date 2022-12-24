using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.HelperTree;

public class ProcedureCollectionNames
{
    public ProcedureCollectionNames(ContractNames contract, EndPointNames endPoint)
    {
        string lowerCaseBase = endPoint.LowerCaseBase;
        string upperCaseBase = endPoint.UpperCaseBase;

        InterfaceType = $"{InterfacePrefix}{upperCaseBase}";
        InterfaceParameter = lowerCaseBase;
        InterfaceField = $"{PrivateFieldPrefix}{lowerCaseBase}";
        
        EnumType = $"{upperCaseBase}{ProcedurePostfix}";
        EnumParameter = $"{lowerCaseBase}{ProcedurePostfix}";        
    }
    
    public string InterfaceType { get; }
    public string InterfaceParameter { get; }
    public string InterfaceField { get; }
    
    /// <summary>
    /// enum type name (local to generated namespace)
    /// </summary>
    public string EnumType { get; }
    public string EnumParameter { get; }
}