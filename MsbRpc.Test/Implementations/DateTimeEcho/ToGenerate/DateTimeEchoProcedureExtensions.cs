namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public static class DateTimeEchoProcedureExtensions
{
    public static string GetName(this DateTimeEchoProcedure procedure)
    {
        return procedure switch
        {
            DateTimeEchoProcedure.GetDateTime => nameof(DateTimeEchoProcedure.GetDateTime),
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    public static int GetId(this DateTimeEchoProcedure procedure)
    {
        return procedure switch
        {
            DateTimeEchoProcedure.GetDateTime => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    public static DateTimeEchoProcedure FromId(int procedureId)
    {
        return procedureId switch
        {
            0 => DateTimeEchoProcedure.GetDateTime,
            _ => throw new ArgumentOutOfRangeException(nameof(procedureId), procedureId, null)
        };
    }
}