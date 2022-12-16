﻿namespace MsbRpcTest.ManualRpcTest.Incrementer.Generated;

public static class IncrementerServerProcedureExtensions
{
    public static string GetName(this IncrementerServerProcedure procedure)
    {
        return procedure switch
        {
            IncrementerServerProcedure.Increment => nameof(IncrementerServerProcedure.Increment),
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    //todo: generated this
    public static bool GetInvertsDirection(this IncrementerServerProcedure procedure)
    {
        return procedure switch
        {
            IncrementerServerProcedure.Increment => false,
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }
}