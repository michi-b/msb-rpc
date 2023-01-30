using System;
using Incrementer;
using Incrementer.Generated;
using MsbRpc.Configuration;
using MsbRpc.Contracts;
using MsbRpc.EndPoints;
using MsbRpc.Exceptions;
using MsbRpc.Messaging;
using MsbRpc.Serialization;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

// ReSharper disable RedundantBaseQualifier

namespace MsbRpc.Test.Generator.Incrementer.ToGenerate;

public class IncrementerServerEndPoint
    : InboundEndPoint<IncrementerServerEndPoint, IncrementerProcedure, IIncrementer>
{
    public IncrementerServerEndPoint
    (
        Messenger messenger,
        IIncrementer implementation,
        // ReSharper disable once SuggestBaseTypeForParameterInConstructor
        Configuration configuration
    ) : base
    (
        messenger,
        implementation,
        configuration
    ) { }

    public class Configuration : InboundEndPointConfiguration { }

    protected override Response Execute(IncrementerProcedure procedure, Request request)
    {
        return procedure switch
        {
            IncrementerProcedure.Increment => Increment(request),
            IncrementerProcedure.IncrementString => IncrementString(request),
            IncrementerProcedure.Store => Store(request),
            IncrementerProcedure.IncrementStored => IncrementStored(),
            IncrementerProcedure.GetStored => GetStored(),
            IncrementerProcedure.Finish => Finish(),
            _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
        };
    }

    private Response Increment(Request request)
    {
        BufferReader requestReader = request.GetReader();

        // Read request arguments.
        int valueArgument = requestReader.ReadInt();

        // Execute procedure.
        int result = Implementation.Increment(valueArgument);

        // Send response.
        const int resultSize = PrimitiveSerializer.IntSize;

        Response response = Buffer.GetResponse(Implementation.RanToCompletion, resultSize);
        BufferWriter responseWriter = response.GetWriter();

        responseWriter.Write(result);

        return response;
    }

    private Response IncrementString(Request request)
    {
        BufferReader requestReader = request.GetReader();

        string valueArgument;
        try
        {
            valueArgument = StringSerializer.Read(requestReader);
        }
        catch (Exception e)
        {
            throw new RpcExecutionException<IncrementerProcedure>(e, IncrementerProcedure.IncrementString, RpcExecutionStage.ArgumentDeserialization);
        }

        string result;
        try
        {
            result = Implementation.IncrementString(valueArgument);
        }
        catch (Exception e)
        {
            throw new RpcExecutionException<IncrementerProcedure>(e, IncrementerProcedure.IncrementString, RpcExecutionStage.ImplementationCall);
        }

        Response response;
        try
        {
            int resultSize = StringSerializer.GetSize(result);

            int responseSize = resultSize + PrimitiveSerializer.IntSize;

            response = Buffer.GetResponse(Implementation.RanToCompletion, responseSize);
            BufferWriter responseWriter = response.GetWriter();

            StringSerializer.Write(result, responseWriter);
        }
        catch (Exception e)
        {
            throw new RpcExecutionException<IncrementerProcedure>(e, IncrementerProcedure.IncrementString, RpcExecutionStage.ResultSerialization);
        }

        return response;
    }

    private Response Store(Request request)
    {
        BufferReader requestReader = request.GetReader();

        int valueArgument = requestReader.ReadInt();

        Implementation.Store(valueArgument);

        return Buffer.GetResponse(Implementation.RanToCompletion);
    }

    private Response IncrementStored()
    {
        Implementation.IncrementStored();
        return Buffer.GetResponse(Implementation.RanToCompletion);
    }

    private Response Finish()
    {
        Implementation.Finish();
        return Buffer.GetResponse(Implementation.RanToCompletion);
    }

    private Response GetStored()
    {
        int result = Implementation.GetStored();

        const int resultSize = PrimitiveSerializer.IntSize;

        Response response = Buffer.GetResponse(Implementation.RanToCompletion, resultSize);
        BufferWriter responseWriter = response.GetWriter();

        responseWriter.Write(result);

        return response;
    }

    protected override IncrementerProcedure GetProcedure(int procedureId) => IncrementerProcedureExtensions.FromId(procedureId);

    protected override string GetName(IncrementerProcedure procedure) => procedure.GetName();
}