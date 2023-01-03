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
            IncrementerProcedure.End => nameof(IncrementerProcedure.End),
            _ => throw new System.ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    public static bool GetClosesConnection(this IncrementerProcedure procedure)
    {
        return procedure switch
        {
            IncrementerProcedure.Increment => false,
            IncrementerProcedure.Store => false,
            IncrementerProcedure.IncrementStored => false,
            IncrementerProcedure.GetStored => false,
            IncrementerProcedure.End => true,
            _ => throw new System.ArgumentOutOfRangeException(nameof(procedure), procedure, null)
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
            IncrementerProcedure.End => 4,
            _ => throw new System.ArgumentOutOfRangeException(nameof(procedure), procedure, null)
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
            4 => IncrementerProcedure.End,
            _ => throw new System.ArgumentOutOfRangeException(nameof(procedureId), procedureId, null)
        };
    }
}