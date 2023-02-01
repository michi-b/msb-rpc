using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Primitives;

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

    private static IEnumerable<string> GetOptionNames(this RpcExceptionTransmissionOptions target)
    {
        if (target.HasTypeName())
        {
            yield return nameof(RpcExceptionTransmissionOptions.TypeName);
        }
        if (target.HasExecutionStage())
        {
            yield return nameof(RpcExceptionTransmissionOptions.ExecutionStage);
        }
        if (target.HasMessage())
        {
            yield return nameof(RpcExceptionTransmissionOptions.Message);
        }
        if (target.HasContinuation())
        {
            yield return nameof(RpcExceptionTransmissionOptions.Continuation);
        }
    }

    public static bool HasTypeName(this RpcExceptionTransmissionOptions target) => (target & RpcExceptionTransmissionOptions.TypeName) != 0;
    
    public static bool HasExecutionStage(this RpcExceptionTransmissionOptions target) => (target & RpcExceptionTransmissionOptions.ExecutionStage) != 0;
    
    public static bool HasMessage(this RpcExceptionTransmissionOptions target) => (target & RpcExceptionTransmissionOptions.Message) != 0;
    
    public static bool HasContinuation(this RpcExceptionTransmissionOptions target) => (target & RpcExceptionTransmissionOptions.Continuation) != 0;
}