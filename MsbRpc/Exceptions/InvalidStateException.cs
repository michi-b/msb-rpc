using System.Runtime.CompilerServices;
using MsbRpc.Utility.Generic;

namespace MsbRpc;

public class InvalidStateException<TEnum> : InvalidOperationException where TEnum : Enum
{
    public InvalidStateException(TEnum expectedState, TEnum actualState, [CallerMemberName] string? operationName = null)
        : base
        (
            "operation "
            + OperationNameOrEmpty(operationName)
            + "is only available in state "
            + $"'{StateUtility<TEnum>.GetName(expectedState)}' "
            + "but was called in state "
            + $"'{StateUtility<TEnum>.GetName(actualState)}'"
        ) { }

    public InvalidStateException(TEnum[] allowedStates, TEnum actualState, [CallerMemberName] string? operationName = null)
        : base
        (
            "operation "
            + OperationNameOrEmpty(operationName)
            + "is only available in states {"
            + string.Join(", ", allowedStates.Select(state => $"'{StateUtility<TEnum>.GetName(state)}'"))
            + "but was called in state "
            + $"'{StateUtility<TEnum>.GetName(actualState)}'"
        ) { }

    private static string OperationNameOrEmpty(string? operationName) => operationName != null ? $"'{operationName}' " : string.Empty;
}