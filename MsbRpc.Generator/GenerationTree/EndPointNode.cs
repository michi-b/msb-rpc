using MsbRpc.Generator.Enums;
using static MsbRpc.Generator.Utility.Names;

namespace MsbRpc.Generator.GenerationTree;

public class EndPointNode
{
    public readonly ContractNode Contract;
    
    /// <summary>
    /// inbound or outbound
    /// </summary>
    public readonly EndPointDirection Direction;
    
    /// <summary>
    /// class name of the endpoint
    /// </summary>
    public readonly string Name;

    public EndPointNode
    (
        ContractNode contract,
        EndPointType type,
        EndPointDirection direction
    )
    {
        Contract = contract;
        string upperCaseTypeId = type.GetName();
        string nameBase = $"{contract.PascalCaseName}{upperCaseTypeId}";
        Name = $"{nameBase}{EndPointPostfix}";
        Direction = direction;
    }
}