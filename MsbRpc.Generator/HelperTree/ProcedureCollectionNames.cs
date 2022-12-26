using MsbRpc.Generator.HelperTree.Names;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.HelperTree;

public class ProcedureCollectionNames
{
    public ProcedureCollectionNames(ContractNames contract, EndPointNames endPoint)
    {
        string lowerCaseBase = endPoint.CamelCaseName;
        string upperCaseBase = endPoint.PascalCaseName;

        InterfaceType = $"{InterfacePrefix}{upperCaseBase}";
        InterfaceParameter = lowerCaseBase;
        InterfaceField = $"{PrivateFieldPrefix}{lowerCaseBase}";
        
        EnumType = $"{upperCaseBase}{ProcedurePostfix}";
    }
    
    public readonly string InterfaceType;
    public readonly string InterfaceParameter;
    public readonly string InterfaceField;

    /// <summary>
    /// enum type name (local to generated namespace)
    /// </summary>
    public readonly string EnumType;
}