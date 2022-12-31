﻿// ReSharper disable once CheckNamespace
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
            _ => throw new System.ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    public static bool GetStopsListening(this IncrementerProcedure procedure)
    {
        return procedure switch
        {
            IncrementerProcedure.Increment => false,
            IncrementerProcedure.Store => false,
            IncrementerProcedure.IncrementStored => false,
            IncrementerProcedure.GetStored => false,
            _ => throw new System.ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    public static IncrementerProcedure GetProcedure(int procedureId)
    {
        return procedureId switch
        {
            0 => IncrementerProcedure.Increment,
            1 => IncrementerProcedure.Store,
            2 => IncrementerProcedure.IncrementStored,
            3 => IncrementerProcedure.GetStored,
            _ => throw new System.ArgumentOutOfRangeException(nameof(procedureId), procedureId, null)
        };
    }
}