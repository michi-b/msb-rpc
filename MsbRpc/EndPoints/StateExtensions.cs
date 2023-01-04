using System.Diagnostics;
using MsbRpc.Utility.Generic;

namespace MsbRpc.EndPoints;

using StateUtility = StateUtility<State>;

public static class StateExtensions
{
    private static readonly State[] RespondingStates = { State.Listening };
    private static readonly State[] RequestingStates = { State.Calling };

    public static void Transition(this ref State target, object stateLock, State stateFrom, State stateTo)
    {
#if DEBUG
        StateUtility.Transition(ref target, stateLock, stateFrom, stateTo);
#else
        target = stateTo;
#endif
    }

    [Conditional("DEBUG")]
    internal static void AssertIs(this State target, State expected)
    {
        StateUtility.AssertIs(expected, target);
    }

    [Conditional("DEBUG")]
    internal static void AssertIsRequesting(this State target)
    {
        target.AssertIsAmong(RequestingStates);
    }

    [Conditional("DEBUG")]
    internal static void AssertIsResponding(this State target)
    {
        target.AssertIsAmong(RespondingStates);
    }

    [Conditional("DEBUG")]
    private static void AssertIsAmong(this State target, params State[] expected)
    {
        StateUtility.AssertIsAmong(expected, target);
    }
}