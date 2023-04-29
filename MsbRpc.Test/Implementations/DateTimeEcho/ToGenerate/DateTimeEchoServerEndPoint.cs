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
        BufferReader requestReader = request.GetReader();

        DateTime myDateTimeArgument = DateTimeSerializer.Read(requestReader);

        DateTime result;

        try
        {
            result = Implementation.GetDateTime(myDateTimeArgument);
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

        Response response;
        try
        {
            response = Buffer.GetResponse(Implementation.RanToCompletion, DateTimeSerializer.Size);

            BufferWriter responseWriter = response.GetWriter();

            DateTimeSerializer.Write(responseWriter, result);
        }
        catch (Exception e)
        {
            throw new RpcExecutionException<DateTimeEchoProcedure>(e, DateTimeEchoProcedure.GetDateTime, RpcExecutionStage.ResponseSerialization);
        }

        return response;
    }

    protected override DateTimeEchoProcedure GetProcedure(int procedureId) => DateTimeEchoProcedureExtensions.FromId(procedureId);

    protected override string GetName(DateTimeEchoProcedure procedure) => procedure.GetName();
}