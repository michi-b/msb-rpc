using MsbRpc.Generator.GenerationTree.Names;
using static MsbRpc.Generator.IndependentNames;

namespace MsbRpc.Generator.GenerationTree;

public class ProcedureCollectionNames
{
    /// <summary>
    ///     enum type name (local to generated namespace)
    /// </summary>
    public readonly string EnumType;
    public ProcedureCollectionNames(EndPointNames endPoint) 
        => EnumType = $"{endPoint.PascalCaseName}{ProcedurePostfix}";
}