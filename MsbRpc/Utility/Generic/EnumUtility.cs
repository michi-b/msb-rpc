using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace MsbRpc.Utility.Generic;

public static class EnumUtility<TEnum> where TEnum : Enum
{
    [PublicAPI]
    public static void Transition
        (ref TEnum state, TEnum stateFrom, TEnum stateTo, AutoResetEvent stateLock, [CallerMemberName] string? operationName = null)
    {
        stateLock.WaitOne();
        try
        {
            Transition(ref state, stateFrom, stateTo, operationName);
        }
        finally
        {
            stateLock.Set();
        }
    }

    [PublicAPI]
    public static void Transition
        (ref TEnum state, TEnum[] allowedStatesFrom, TEnum stateTo, AutoResetEvent stateLock, [CallerMemberName] string? operationName = null)
    {
        stateLock.WaitOne();
        try
        {
            Transition(ref state, allowedStatesFrom, stateTo, operationName);
        }
        finally
        {
            stateLock.Set();
        }
    }
    
    [PublicAPI]
    public static void Transition(ref TEnum state, TEnum[] allowedStatesFrom, TEnum stateTo, [CallerMemberName] string? operationName = null)
    {
        if (allowedStatesFrom.Contains(state))
        {
            state = stateTo;
        }
        else
        {
            throw new InvalidStateException<TEnum>(allowedStatesFrom, state, operationName);
        }
    }
    
    [PublicAPI]
    public static void Transition(ref TEnum state, TEnum stateFrom, TEnum stateTo, [CallerMemberName] string? operationName = null)
    {
        if (state.Equals(stateFrom))
        {
            state = stateTo;
        }
        else
        {
            throw new InvalidStateException<TEnum>(stateFrom, state, operationName);
        }
    }

    public static string GetName
        (TEnum value) =>
        Enum.GetName(typeof(TEnum), value) ?? throw new ArgumentException($"value must be fo type {typeof(TEnum).FullName}", nameof(value));
}