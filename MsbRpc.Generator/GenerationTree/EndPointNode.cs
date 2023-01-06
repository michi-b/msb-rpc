using MsbRpc.Generator.Enums;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class EndPointNode
{
    public readonly ContractNode ContractNode;
    public readonly EndPointType Type;
    public readonly string PascalCaseName;
    public readonly string CamelCaseName;
    public readonly string ImplementationInterface;
    public const string DefaultBufferSizeConstant = "DefaultBufferSize";
    
    public EndPointNode
    (
        ContractNode contract,
        EndPointType endPointType
    )
    {
        ContractNode = contract;
        Type = endPointType;
        string upperCaseTypeId = Type.GetPostfix();
        PascalCaseName = $"{contract.PascalCaseName}{upperCaseTypeId}";
        CamelCaseName = $"{contract.CamelCaseName}{upperCaseTypeId}";
        ImplementationInterface = $"{InterfacePrefix}{PascalCaseName}{ImplementationPostfix}";
    }
    
    
}