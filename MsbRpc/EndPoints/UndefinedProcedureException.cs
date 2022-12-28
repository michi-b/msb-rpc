#nullable enable
using System;
using System.Runtime.CompilerServices;

namespace MsbRpc.EndPoints;

public class UndefinedProcedureException<TInboundProcedure, TOutboundProcedure> : InvalidOperationException where TInboundProcedure : Enum
    where TOutboundProcedure : Enum
{
    private const string NoProceduresInBothDirectionsMessage =
        "Invalid call to method '{0}' of endpoint with type '{1}' because ther are no procedures defined in either direction.";

    private const string NoInboundProceduresMessage =
        "Invalid call to method '{0}' of endpoint with type '{1}' because ther are no inbound procedures defined.";

    private const string NoOutboundProceduresMessage =
        "Invalid call to method '{0}' of endpoint with type '{1}' because ther are no outbound procedures defined.";

    public UndefinedProcedureException
        (RpcEndPoint<TInboundProcedure, TOutboundProcedure> endPoint, [CallerMemberName] string? callerMemberName = null) : base
        (GetMessage(endPoint, callerMemberName!)) { }

    private static string GetMessage
        (RpcEndPoint<TInboundProcedure, TOutboundProcedure> endPoint, string callerMemberName)
        => string.Format(GetMessageTemplate(), callerMemberName, endPoint.GetType().FullName);

    private static string GetMessageTemplate()
        => typeof(TInboundProcedure) == typeof(UndefinedProcedure)
            ? typeof(TOutboundProcedure) == typeof(UndefinedProcedure)
                ? NoProceduresInBothDirectionsMessage
                : NoInboundProceduresMessage
            : typeof(TOutboundProcedure) == typeof(UndefinedProcedure)
                ? NoOutboundProceduresMessage
                : throw new InvalidOperationException("undefined procedure exception should not be thrown if both directions are defined");
}