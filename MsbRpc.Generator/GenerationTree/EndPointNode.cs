#region

using MsbRpc.Generator.CodeWriters.Files.Base;
using MsbRpc.Generator.Enums;

#endregion

namespace MsbRpc.Generator.GenerationTree;

internal class EndPointNode
{
    private const string EndPointPostfix = "EndPoint";
    public readonly string ConfigurationBuilderFullName;

    public readonly string ConfigurationBuilderName;

    public readonly ContractNode Contract;

    /// <summary>
    ///     inbound or outbound
    /// </summary>
    public readonly EndPointDirection Direction;

    /// <summary>
    ///     class name of the endpoint prepended with the namespace
    /// </summary>
    public readonly string FullName;

    /// <summary>
    ///     class name of the endpoint
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

        // names
        Name = $"{nameBase}{EndPointPostfix}";
        FullName = $"{contract.Namespace}.{Name}";
        ConfigurationBuilderName = $"{Name}{ConfigurationBuilderWriter.ConfigurationBuilderPostfix}";
        ConfigurationBuilderFullName = $"{FullName}{ConfigurationBuilderWriter.ConfigurationBuilderPostfix}";

        Direction = direction;
    }
}