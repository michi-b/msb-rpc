using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using MsbRpc.Exceptions;

namespace MsbRpc.Utility.Generic;

public static class StateUtility<TState> where TState : Enum
{
    [PublicAPI]
    public static void Transition
    (
        ref TState state,
        TState stateFrom,
        TState stateTo,
        AutoResetEvent stateLock,
        Action? action = null,
        [CallerMemberName] string? operationName = null
    )
    {
        stateLock.WaitOne();
        try
        {
            action?.Invoke();
            Transition(ref state, stateFrom, stateTo, action, operationName);
        }
        finally
        {
            stateLock.Set();
        }
    }

    [PublicAPI]
    public static void Transition
    (
        ref TState state,
        TState[] allowedStatesFrom,
        TState stateTo,
        AutoResetEvent stateLock,
        Action? action = null,
        [CallerMemberName] string? operationName = null
    )
    {
        stateLock.WaitOne();
        try
        {
            Transition(ref state, allowedStatesFrom, stateTo, action, operationName);
        }
        finally
        {
            stateLock.Set();
        }
    }

    [PublicAPI]
    public static void Transition
    (
        ref TState state,
        TState[] allowedStatesFrom,
        TState stateTo,
        Action? action = null,
        [CallerMemberName] string? operationName = null
    )
    {
        AssertIsAmong(allowedStatesFrom, state, operationName);
        action?.Invoke();
        state = stateTo;
    }

    [PublicAPI]
    public static void Transition
    (
        ref TState state,
        TState stateFrom,
        TState stateTo,
        Action? action = null,
        [CallerMemberName] string? operationName = null
    )
    {
        AssertIs(state, stateFrom);
        action?.Invoke();
        state = stateTo;
    }

    [PublicAPI]
    [Conditional("DEBUG")]
    public static void AssertIs(TState expected, TState actual, [CallerMemberName] string? operationName = null)
    {
        if (!actual.Equals(expected))
        {
            throw new InvalidStateException<TState>(expected, actual, operationName);
        }
    }

    [PublicAPI]
    [Conditional("DEBUG")]
    public static void AssertIsAmong(TState[] expected, TState actual, [CallerMemberName] string? operationName = null)
    {
        if (!expected.Contains(actual))
        {
            throw new InvalidStateException<TState>(expected, actual, operationName);
        }
    }

    public static string GetName
        (TState value) =>
        Enum.GetName(typeof(TState), value) ?? throw new ArgumentException($"value must be fo type {typeof(TState).FullName}", nameof(value));
}