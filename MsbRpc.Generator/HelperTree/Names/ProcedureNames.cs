using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree.Names;

public class ProcedureNames
{
    public string EnumValue { get; }
    public string Name { get; }
    
    public ProcedureNames(ProcedureCollectionNames collectionNames, ProcedureInfo info)
    {
        Name = info.Name;
        EnumValue = $"{collectionNames.EnumType}.{Name}";
    }
}