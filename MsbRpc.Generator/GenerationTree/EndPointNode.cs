using MsbRpc.Generator.Enums;
using static MsbRpc.Generator.CodeWriters.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

internal class EndPointNode
{
    public readonly string ConfigurationTypeFullName;
    public readonly ContractNode Contract;
    public readonly EndPointDirection Direction;
    public readonly string EndPointName;
    public readonly ConnectionEndType EndType;
    public readonly string PascalCaseName;

    public EndPointNode
    (
        ContractNode contract,
        ConnectionEndType connectionEndType,
        EndPointDirection direction
    )
    {
        Contract = contract;
        EndType = connectionEndType;
        string upperCaseTypeId = EndType.GetName();
        PascalCaseName = $"{contract.PascalCaseName}{upperCaseTypeId}";
        EndPointName = $"{PascalCaseName}{EndPointPostfix}";
        Direction = direction;
        ConfigurationTypeFullName = $"{contract.Namespace}.{EndPointName}.{Types.LocalConfiguration}";
    }
}