namespace MsbRpc;

public class InvalidStateException<TEnum> : InvalidOperationException where TEnum : Enum
{
    public InvalidStateException(TEnum expectedState, TEnum actualState, string? operationName = null)
        : base
        (
            "operation "
            + (operationName != null ? $"'{operationName}' " : string.Empty) 
            + "is only available in state "
            + $"'{Enum.GetName(typeof(TEnum), expectedState)}' "
            + "but was called in state "
            + $"'{Enum.GetName(typeof(TEnum), actualState)}'"
        ) { }
}