using MsbRpc.Generator.Enums;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class EndPointNode
{
    public readonly string CamelCaseName;
    public readonly ContractNode Contract;
    public readonly string EndPointName;
    public readonly string ImplementationInterface;
    public readonly string PascalCaseName;
    public readonly EndPointType Type;

    public EndPointNode
    (
        ContractNode contract,
        EndPointType endPointType
    )
    {
        Contract = contract;
        Type = endPointType;
        string upperCaseTypeId = Type.GetPostfix();
        PascalCaseName = $"{contract.PascalCaseName}{upperCaseTypeId}";
        CamelCaseName = $"{contract.CamelCaseName}{upperCaseTypeId}";
        ImplementationInterface = $"{InterfacePrefix}{PascalCaseName}{ImplementationPostfix}";
        EndPointName = $"{PascalCaseName}{EndPointPostfix}";
    }
}