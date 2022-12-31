#nullable enable
using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;
using MsbRpc.Exceptions;

namespace MsbRpc.Utility.Generic;

public static class StateUtility<TState> where TState : Enum
{
    [PublicAPI]
    public static void Transition
    (
        ref TState state,
        AutoResetEvent stateLock,
        TState[] allowedStatesFrom,
        TState stateTo,
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
        AutoResetEvent stateLock,
        TState stateFrom,
        TState stateTo,
        Action? action = null,
        [CallerMemberName] string? operationName = null
    )
    {
        stateLock.WaitOne();
        try
        {
            Transition(ref state, stateFrom, stateTo, action, operationName);
        }
        finally
        {
            stateLock.Set();
        }
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
        if (!((IList)expected).Contains(actual))
        {
            throw new InvalidStateException<TState>(expected, actual, operationName);
        }
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
        AssertIs(state, stateFrom, operationName);
        action?.Invoke();
        state = stateTo;
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
        object stateLock,
        TState stateFrom,
        TState stateTo,
        Action? action = null,
        [CallerMemberName] string? operationName = null
    )
    {
        lock (stateLock)
        {
            AssertIs(state, stateFrom, operationName);
            action?.Invoke();
            state = stateTo;
        }
    }

    [PublicAPI]
    public static void Transition
    (
        ref TState state,
        object stateLock,
        TState[] allowedStatesFrom,
        TState stateTo,
        Action? action = null,
        [CallerMemberName] string? operationName = null
    )
    {
        lock (stateLock)
        {
            AssertIsAmong(allowedStatesFrom, state, operationName);
            action?.Invoke();
            state = stateTo;
        }
    }
}