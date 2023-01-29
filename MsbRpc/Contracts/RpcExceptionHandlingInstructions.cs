namespace MsbRpc.Contracts;

public struct RpcExceptionHandlingInstructions
{
    public bool Log { get; set; }

    public RpcExceptionTransmissionOptions TransmissionOptions { get; set; }
    
    public RpcExceptionContinuation Continuation { get; set; }

    public static readonly RpcExceptionHandlingInstructions Default = new()
    {
        TransmissionOptions = RpcExceptionTransmissionOptions.None,
        Log = true,
        Continuation = RpcExceptionContinuation.Dispose
    };
}