using System.Diagnostics;
using StateUtility = MsbRpc.Utility.Generic.StateUtility<MsbRpc.EndPoints.State>;

namespace MsbRpc.EndPoints;

public static class StateExtensions
{
    [Conditional("DEBUG")]
    internal static void AssertIs(this State target, State expected)
    {
        StateUtility.AssertIs(expected, target);
    }

    [Conditional("DEBUG")]
    internal static void AssertIsRequesting(this State target)
    {
        target.AssertIsAmong(_requestingStates);
    }
    
    [Conditional("DEBUG")]
    internal static void AssertIsResponding(this State target)
    {
        target.AssertIsAmong(_respondingStates);
    }
    
    [Conditional("DEBUG")]
    private static void AssertIsAmong(this State target, params State[] expected)
    {
        StateUtility.AssertIsAmong(expected, target);
    }

    public static void Transition(this ref State target, State stateFrom, State stateTo)
    {
#if DEBUG
        StateUtility.Transition(ref target, stateFrom, stateTo);
#else
        target = stateTo;
#endif
    }

    private static State[] _respondingStates = new[] { State.Listening };
    private static State[] _requestingStates = new[] { State.Calling };
}