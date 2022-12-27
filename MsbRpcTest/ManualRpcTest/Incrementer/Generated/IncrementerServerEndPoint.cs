using Microsoft.CodeAnalysis.CSharp.Syntax;
using MsbRpc.EndPoints;
using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;

namespace MsbRpcTest.ManualRpcTest.Incrementer.Generated;

public class IncrementerServerEndPoint : MsbRpc.EndPoints.RpcEndPoint<IncrementerServerProcedure, MsbRpc.EndPoints.UndefinedProcedure>
{
    public IncrementerServerEndPoint
    (
        MsbRpc.Messaging.Messenger messenger,
        Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null,
        int initialBufferSize = DefaultBufferSize
    ) : base
        (
            messenger,
            MsbRpc.EndPoints.EndPointDirection.Inbound,
            loggerFactory != null
                ? Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger<IncrementerServerEndPoint>(loggerFactory)
                : new Microsoft.Extensions.Logging.Abstractions.NullLogger<IncrementerServerEndPoint>(),
            initialBufferSize
        ) { }

    protected override string GetName(IncrementerServerProcedure procedure) => procedure.GetName();

    protected override bool GetInvertsDirection(IncrementerServerProcedure procedure) => procedure.GetInvertsDirection();

    public Messenger.ListenReturnCode Listen(IIncrementerServerImplementation implementation) => base.Listen(CreateResolver(implementation));
    
    public Resolver CreateResolver(IIncrementerServerImplementation implementation)
    {
        return new Resolver(this, implementation);
    }

    public class Resolver : MsbRpc.EndPoints.IRpcResolver<IncrementerServerProcedure>
    {
        private readonly IncrementerServerEndPoint _endPoint;
        private readonly IIncrementerServerImplementation _implementation;
        
        public Resolver(IncrementerServerEndPoint endPoint, IIncrementerServerImplementation implementation)
        {
            _endPoint = endPoint;
            _implementation = implementation;
        }

        public BufferWriter Resolve(IncrementerServerProcedure procedure, BufferReader arguments)
        {
            return procedure switch
            {
                IncrementerServerProcedure.Increment => Increment(arguments),
                _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
            };   
        }
        private MsbRpc.Serialization.Buffers.BufferWriter Increment(MsbRpc.Serialization.Buffers.BufferReader argumentsReader)
        {
            // Read request arguments.
            int valueArgument = argumentsReader.ReadInt();

            // Execute procedure.
            int result = _implementation.Increment(valueArgument);

            // Send response.
            const int resultSize = MsbRpc.Serialization.Primitives.PrimitiveSerializer.IntSize;

            MsbRpc.Serialization.Buffers.BufferWriter responseWriter = _endPoint.GetResponseWriter(resultSize);

            responseWriter.Write(result);

            return responseWriter;
        }
    }
}