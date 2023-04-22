using MsbRpc.Configuration;
using MsbRpc.Contracts;
using MsbRpc.EndPoints;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

// ReSharper disable UnusedParameter.Local

namespace MsbRpc.Test.Implementations.DateTimeEcho.ToGenerate;

public class DateTimeEchoServerEndPoint
    : InboundEndPoint<DateTimeEchoProcedure, IDateTimeEcho>
{
    public DateTimeEchoServerEndPoint
    (
        Messenger messenger,
        IDateTimeEcho implementation,
        InboundEndPointConfiguration configuration
    ) : base
    (
        messenger,
        implementation,
        configuration
    ) { }

    protected override Response Execute
    (
        DateTimeEchoProcedure procedure,
        Request request
    )
    {
        return procedure switch
        {
            DateTimeEchoProcedure.GetDateTime => GetDateTime(request),
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    private Response GetDateTime(Request request)
    {
        DateTime myDateTimeArgument;

        myDateTimeArgument = default!;

        try
        {
            Implementation.GetDateTime(myDateTimeArgument);
        }
        catch (Exception exception)
        {
            throw new RpcExecutionException<DateTimeEchoProcedure>
            (
                exception,
                DateTimeEchoProcedure.GetDateTime,
                RpcExecutionStage.ImplementationExecution
            );
        }

        return Buffer.GetResponse(Implementation.RanToCompletion);
    }

    protected override DateTimeEchoProcedure GetProcedure(int procedureId) => DateTimeEchoProcedureExtensions.FromId(procedureId);

    protected override string GetName(DateTimeEchoProcedure procedure) => procedure.GetName();
}