// ReSharper disable once CheckNamespace

using System;

namespace Incrementer.Generated;

public static class IncrementerProcedureExtensions
{
    public static string GetName(this IncrementerProcedure procedure)
    {
        return procedure switch
        {
            IncrementerProcedure.Increment => nameof(IncrementerProcedure.Increment),
            IncrementerProcedure.Store => nameof(IncrementerProcedure.Store),
            IncrementerProcedure.IncrementStored => nameof(IncrementerProcedure.IncrementStored),
            IncrementerProcedure.GetStored => nameof(IncrementerProcedure.GetStored),
            IncrementerProcedure.Finish => nameof(IncrementerProcedure.Finish),
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    public static int GetId(this IncrementerProcedure procedure)
    {
        return procedure switch
        {
            IncrementerProcedure.Increment => 0,
            IncrementerProcedure.Store => 1,
            IncrementerProcedure.IncrementStored => 2,
            IncrementerProcedure.GetStored => 3,
            IncrementerProcedure.Finish => 4,
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    public static IncrementerProcedure FromId(int procedureId)
    {
        return procedureId switch
        {
            0 => IncrementerProcedure.Increment,
            1 => IncrementerProcedure.Store,
            2 => IncrementerProcedure.IncrementStored,
            3 => IncrementerProcedure.GetStored,
            4 => IncrementerProcedure.Finish,
            _ => throw new ArgumentOutOfRangeException(nameof(procedureId), procedureId, null)
        };
    }
}