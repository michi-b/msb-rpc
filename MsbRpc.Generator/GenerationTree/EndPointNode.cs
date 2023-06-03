using MsbRpc.Generator.Enums;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.GenerationTree;

public class EndPointNode
{
    public readonly ContractNode Contract;
    public readonly EndPointDirection Direction;
    public readonly string Name;
    private readonly EndPointType Type;

    public EndPointNode
    (
        ContractNode contract,
        EndPointType type,
        EndPointDirection direction
    )
    {
        Contract = contract;
        Type = type;
        string upperCaseTypeId = Type.GetName();
        string nameBase = $"{contract.PascalCaseName}{upperCaseTypeId}";
        Name = $"{nameBase}{EndPointPostfix}";
        Direction = direction;
    }
}