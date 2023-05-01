using System.Collections.Generic;
using MsbRpc.Generator.Info;
using static MsbRpc.Generator.Serialization.SerializationWriter;

namespace MsbRpc.Generator.Serialization;

public class GenericSerializationWriterFactory
{
    public delegate SizeExpressionFactory SizeExpressionFactoryFactory(IList<SerializationWriter> typeArgumentSerializationWriters);

    public delegate SerializationStatementFactory SerializationStatementFactoryFactory(IList<SerializationWriter> typeArgumentSerializationWriters);

    public delegate DeserializationExpressionFactory DeserializationExpressionFactoryFactory(IList<SerializationWriter> typeArgumentSerializationWriters);

    private readonly DeserializationExpressionFactoryFactory _deserializationExpressionFactoryFactory;
    private readonly SerializationStatementFactoryFactory _serializationStatementFactoryFactory;
    private readonly SizeExpressionFactoryFactory _sizeExpressionFactoryFactory;

    public GenericSerializationWriterFactory
    (
        SizeExpressionFactoryFactory sizeExpressionFactoryFactory,
        DeserializationExpressionFactoryFactory deserializationExpressionFactoryFactory,
        SerializationStatementFactoryFactory serializationStatementFactoryFactory
    )
    {
        _sizeExpressionFactoryFactory = sizeExpressionFactoryFactory;
        _deserializationExpressionFactoryFactory = deserializationExpressionFactoryFactory;
        _serializationStatementFactoryFactory = serializationStatementFactoryFactory;
    }

    public bool TryInstantiate(SerializationResolver resolver, IList<TypeInfo> typeArguments, out SerializationWriter serializationWriter)
    {
        var typeArgumentSerializationWriters = new SerializationWriter[typeArguments.Count];
        for (int i = 0; i < typeArguments.Count; i++)
        {
            TypeInfo type = typeArguments[i];
            if (resolver.TryGetSerializationWriter(type, out SerializationWriter genericArgumentSerializationWriter))
            {
                typeArgumentSerializationWriters[i] = genericArgumentSerializationWriter;
            }
            else
            {
                serializationWriter = new SerializationWriter();
                return false;
            }
        }

        SizeExpressionFactory sizeExpressionFactory = _sizeExpressionFactoryFactory.Invoke(typeArgumentSerializationWriters);
        DeserializationExpressionFactory deserializationExpressionFactory = _deserializationExpressionFactoryFactory.Invoke(typeArgumentSerializationWriters);
        SerializationStatementFactory serializationStatementFactory = _serializationStatementFactoryFactory.Invoke(typeArgumentSerializationWriters);

        serializationWriter = new SerializationWriter(sizeExpressionFactory, serializationStatementFactory, deserializationExpressionFactory);
        return true;
    }
}