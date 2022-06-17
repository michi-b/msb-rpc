using MsbRpc.Serialization.Primitives;

namespace MsbRpc.Serialization;

public class ByteMessageReader
{
    private const int DefaultInitialBufferSize = 128;
    private readonly byte[] _buffer;
    private readonly Stream _stream;

    public ByteMessageReader(Stream stream, uint initialBufferSize = DefaultInitialBufferSize)
    {
        _stream = stream;
        _buffer = new byte[DefaultInitialBufferSize];
    }

    public async Task Run(CancellationToken cancellationToken)
    {
        while (true)
        {
            ReadResult readResult = await Read(sizeof(uint), cancellationToken);
            if (readResult == ReadResult.Success)
            {
                int messageSize = PrimitiveSerializer.ReadInt32(_buffer);
                readResult = await Read(messageSize, cancellationToken);
                if (readResult == ReadResult.Success)
                {
                    //todo: handleMessage
                }
            }
            else
            {
                return;
            }
        }
    }

    private async Task<ReadResult> Read(int count, CancellationToken cancellationToken)
    {
        int sumBytesRead = 0;
        while (sumBytesRead < count)
        {
            int bytesRead = await _stream.ReadAsync(_buffer, sumBytesRead, count - sumBytesRead, cancellationToken);
            if (cancellationToken.IsCancellationRequested)
            {
                return ReadResult.Canceled;
            }

            if (bytesRead == 0)
            {
                return ReadResult.EndOfStream;
            }

            sumBytesRead += bytesRead;
        }

        return ReadResult.Success;
    }

    private enum ReadResult
    {
        Success,
        Canceled,
        EndOfStream
    }
}