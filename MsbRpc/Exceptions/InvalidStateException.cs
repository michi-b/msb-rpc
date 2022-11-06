using System.Runtime.CompilerServices;

namespace MsbRpc.Exceptions;

public class InvalidStateException<TEnum> : InvalidOperationException where TEnum : Enum
{
    public InvalidStateException(TEnum expectedState, TEnum actualState, [CallerMemberName] string? operationName = null)
        : base
        (
            "operation "
            + OperationNameOrEmpty(operationName)
            + "is only available in state "
            + $"'{Enum.GetName(typeof(TEnum), expectedState) ?? throw new ArgumentException($"value must be fo type {typeof(TEnum).FullName}", nameof(expectedState))}' "
            + "but was called in state "
            + $"'{(string)(Enum.GetName(typeof(TEnum), actualState) ?? throw new ArgumentException($"value must be fo type {typeof(TEnum).FullName}", nameof(actualState)))}'"
        ) { }

    public InvalidStateException(TEnum[] allowedStates, TEnum actualState, [CallerMemberName] string? operationName = null)
        : base
        (
            "operation "
            + OperationNameOrEmpty(operationName)
            + "is only available in states {"
            + string.Join
            (
                ", ",
                allowedStates.Select
                (
                    state
                        => $"'{(string)(Enum.GetName(typeof(TEnum), state) ?? throw new ArgumentException($"value must be fo type {typeof(TEnum).FullName}", nameof(state)))}'"
                )
            )
            + "but was called in state "
            + $"'{(string)(Enum.GetName(typeof(TEnum), actualState) ?? throw new ArgumentException($"value must be fo type {typeof(TEnum).FullName}", nameof(actualState)))}'"
        ) { }

    private static string OperationNameOrEmpty(string? operationName) => operationName != null ? $"'{operationName}' " : string.Empty;
}