using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationTree.Names;

public class ProcedureNames
{
    public readonly string Async;
    public readonly string EnumValue;
    public readonly string Name;

    public ProcedureNames(ProcedureCollectionNames collectionNames, ProcedureInfo info)
    {
        Name = info.Name;
        Async = $"{Name}{IndependentNames.AsyncPostFix}";
        EnumValue = $"{collectionNames.EnumType}.{Name}";
    }
}