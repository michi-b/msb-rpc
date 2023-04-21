using Incrementer.Generated;

namespace MsbRpc.Test.Implementations.Incrementer.ToGenerate;

public static class IncrementerProcedureExtensions
{
    public static string GetName(this IncrementerProcedure procedure)
    {
        return procedure switch
        {
            IncrementerProcedure.Increment => nameof(IncrementerProcedure.Increment),
            IncrementerProcedure.IncrementString => nameof(IncrementerProcedure.IncrementString),
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
            IncrementerProcedure.IncrementString => 1,
            IncrementerProcedure.Store => 2,
            IncrementerProcedure.IncrementStored => 3,
            IncrementerProcedure.GetStored => 4,
            IncrementerProcedure.Finish => 5,
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    public static IncrementerProcedure FromId(int procedureId)
    {
        return procedureId switch
        {
            0 => IncrementerProcedure.Increment,
            1 => IncrementerProcedure.IncrementString,
            2 => IncrementerProcedure.Store,
            3 => IncrementerProcedure.IncrementStored,
            4 => IncrementerProcedure.GetStored,
            5 => IncrementerProcedure.Finish,
            _ => throw new ArgumentOutOfRangeException(nameof(procedureId), procedureId, null)
        };
    }
}