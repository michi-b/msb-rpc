using MsbRpc.Messaging;
using MsbRpc.Serialization.Buffers;
using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Test.Generator.Incrementer.Generated;

public class IncrementerServerEndPoint : EndPoints.RpcEndPoint<IncrementerServerProcedure, EndPoints.UndefinedProcedure>
{
    public IncrementerServerEndPoint
    (
        Messenger messenger,
        Microsoft.Extensions.Logging.ILoggerFactory? loggerFactory = null,
        int initialBufferSize = DefaultBufferSize
    ) : base
        (
            messenger,
            EndPoints.EndPointDirection.Inbound,
            loggerFactory != null
                ? Microsoft.Extensions.Logging.LoggerFactoryExtensions.CreateLogger<IncrementerServerEndPoint>(loggerFactory)
                : new Microsoft.Extensions.Logging.Abstractions.NullLogger<IncrementerServerEndPoint>(),
            initialBufferSize
        ) { }

    protected override string GetName(IncrementerServerProcedure procedure) => procedure.GetName();

    protected override bool GetInvertsDirection(IncrementerServerProcedure procedure) => procedure.GetInvertsDirection();

    public Messenger.ListenReturnCode Listen(IIncrementerServerImplementation implementation) => base.Listen(CreateResolver(implementation));
    
    public Resolver CreateResolver(IIncrementerServerImplementation implementation) => new Resolver(this, implementation);

    public class Resolver : EndPoints.IRpcResolver<IncrementerServerProcedure>
    {
        private readonly IncrementerServerEndPoint _endPoint;
        private readonly IIncrementerServerImplementation _implementation;
        
        public Resolver(IncrementerServerEndPoint endPoint, IIncrementerServerImplementation implementation)
        {
            _endPoint = endPoint;
            _implementation = implementation;
        }

        ArraySegment<byte> EndPoints.IRpcResolver<IncrementerServerProcedure>.Execute(IncrementerServerProcedure procedure, BufferReader arguments)
        {
            return procedure switch
            {
                IncrementerServerProcedure.Increment => Increment(arguments),
                IncrementerServerProcedure.Store => Store(arguments),
                IncrementerServerProcedure.IncrementStored => IncrementStored(arguments),
                IncrementerServerProcedure.GetStored => GetStored(arguments),
                _ => throw new ArgumentOutOfRangeException(nameof(procedure), procedure, null)
            };   
        }

        private ArraySegment<byte> GetStored(BufferReader arguments)
        {
            int result = _implementation.GetStored();

            const int resultSize = PrimitiveSerializer.IntSize;

            BufferWriter responseWriter = _endPoint.GetResponseWriter(resultSize);

            responseWriter.Write(result);

            return responseWriter.Buffer;
        }

        private ArraySegment<byte> Increment(BufferReader argumentsReader)
        {
            // Read request arguments.
            int valueArgument = argumentsReader.ReadInt();

            // Execute procedure.
            int result = _implementation.Increment(valueArgument);

            // Send response.
            const int resultSize = Serialization.Primitives.PrimitiveSerializer.IntSize;

            BufferWriter responseWriter = _endPoint.GetResponseWriter(resultSize);

            responseWriter.Write(result);

            return responseWriter.Buffer;
        }

        private ArraySegment<byte> Store(BufferReader arguments)
        {
            int valueArgument = arguments.ReadInt();
            
            _implementation.Store(valueArgument);

            return MsbRpc.Serialization.Buffers.BufferUtility.Empty;
        }

        private ArraySegment<byte> IncrementStored(BufferReader arguments)
        {
            _implementation.IncrementStored();

            return MsbRpc.Serialization.Buffers.BufferUtility.Empty;
        }
    }
}