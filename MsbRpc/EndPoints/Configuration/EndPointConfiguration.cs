namespace MsbRpc.EndPoints.Configuration;

public class EndPointConfiguration : Configuration
{
    public const int DefaultInitialSize = 1024;
    public int InitialBufferSize = DefaultInitialSize;
}