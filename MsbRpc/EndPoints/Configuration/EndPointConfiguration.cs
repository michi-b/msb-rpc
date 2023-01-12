namespace MsbRpc.EndPoints.Configuration;

public class EndPointConfiguration : Configuration
{
    public int InitialBufferSize = DefaultInitialSize;
    
    public const int DefaultInitialSize = 1024;
}