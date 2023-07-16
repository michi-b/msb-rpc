#region

using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace MsbRpc.Exceptions;

public static class RpcExceptionTransmissionOptionsExtensions
{
    public static string GetString(this RpcExceptionTransmissionOptions target)
    {
        string[] optionNames = target.GetOptionNames().ToArray();
        switch (optionNames.Length)
        {
            case 0:
                return "transmit nothing";
            case 1:
                return $"transmit {optionNames[0]}";
            default:
                StringBuilder stringBuilder = new(100);
                stringBuilder.Append($"transmit {optionNames[0]}");
                for (int i = 1; i < optionNames.Length - 1; i++)
                {
                    stringBuilder.Append($", {optionNames[i]}");
                }

                stringBuilder.Append($" and {optionNames[optionNames.Length - 1]}");
                return stringBuilder.ToString();
        }
    }

    public static bool HasTypeName(this RpcExceptionTransmissionOptions target) => (target & RpcExceptionTransmissionOptions.ExceptionTypeName) != 0;

    public static bool HasExecutionStage(this RpcExceptionTransmissionOptions target) => (target & RpcExceptionTransmissionOptions.SourceExecutionStage) != 0;

    public static bool HasMessage(this RpcExceptionTransmissionOptions target) => (target & RpcExceptionTransmissionOptions.ExceptionMessage) != 0;

    public static bool HasContinuation(this RpcExceptionTransmissionOptions target) => (target & RpcExceptionTransmissionOptions.RemoteContinuation) != 0;

    private static IEnumerable<string> GetOptionNames(this RpcExceptionTransmissionOptions target)
    {
        if (target.HasTypeName())
        {
            yield return nameof(RpcExceptionTransmissionOptions.ExceptionTypeName);
        }

        if (target.HasExecutionStage())
        {
            yield return nameof(RpcExceptionTransmissionOptions.SourceExecutionStage);
        }

        if (target.HasMessage())
        {
            yield return nameof(RpcExceptionTransmissionOptions.ExceptionMessage);
        }

        if (target.HasContinuation())
        {
            yield return nameof(RpcExceptionTransmissionOptions.RemoteContinuation);
        }
    }
}