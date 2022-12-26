﻿using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.HelperTree.Names;

public class ProcedureNames
{
    public readonly string CallMethod;
    public readonly string EnumValue;
    public readonly string PascalCaseName;

    public ProcedureNames(ProcedureCollectionNames collectionNames, ProcedureInfo info)
    {
        PascalCaseName = info.Name;
        CallMethod = $"{PascalCaseName}{IndependentNames.AsyncPostFix}";
        EnumValue = $"{collectionNames.EnumType}.{PascalCaseName}";
    }
}