using System.Runtime.CompilerServices;

namespace MsbRpc.Utility;

public static class EnumUtility<TEnum> where TEnum : Enum
{

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
}