namespace MsbRpc.EndPoints;

public class UndefinedProcedureException : InvalidOperationException
{
    public UndefinedProcedureException() : base($"There is no procedure defined for this operation") { }
}