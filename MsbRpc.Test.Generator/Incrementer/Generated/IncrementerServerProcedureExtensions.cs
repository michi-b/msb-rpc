namespace MsbRpc.Test.Generator.Incrementer.Generated;

public static class IncrementerServerProcedureExtensions
{
    public static string GetName(this IncrementerServerProcedure procedure)
    {
        return procedure switch
        {
            IncrementerServerProcedure.Increment => nameof(IncrementerServerProcedure.Increment),
            IncrementerServerProcedure.Store => nameof(IncrementerServerProcedure.Store),
            IncrementerServerProcedure.IncrementStored => nameof(IncrementerServerProcedure.IncrementStored),
            IncrementerServerProcedure.GetStored => nameof(IncrementerServerProcedure.GetStored),
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    public static bool GetInvertsDirection(this IncrementerServerProcedure procedure)
    {
        return procedure switch
        {
            IncrementerServerProcedure.Increment => false,
            IncrementerServerProcedure.Store => false,
            IncrementerServerProcedure.IncrementStored => false,
            IncrementerServerProcedure.GetStored => false,
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }
}