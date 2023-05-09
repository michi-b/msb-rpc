using MsbRpc.Generator.Enums;
using static MsbRpc.Generator.Utility.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

public class EndPointNode
{
    public readonly ContractNode Contract;
    public readonly EndPointDirection Direction;
    public readonly ConnectionEndType EndType;
    public readonly string Name;

    /// <summary>
    ///     endpoint name without "EndPoint" postfix in PascalCase
    /// </summary>
    public readonly string NameBase;

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
        NameBase = $"{contract.PascalCaseName}{upperCaseTypeId}";
        Name = $"{NameBase}{EndPointPostfix}";
        Direction = direction;
    }
}