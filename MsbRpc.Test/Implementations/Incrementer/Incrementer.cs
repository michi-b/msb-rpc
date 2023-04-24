using Incrementer;
using MsbRpc.Contracts;
using MsbRpc.Exceptions;

namespace MsbRpc.Test.Implementations.Incrementer;

public class Incrementer : RpcContractImplementation, IIncrementer
{
    private readonly RpcExceptionTransmissionOptions _exceptionTransmission;
    private int _value;

    public Incrementer(RpcExceptionTransmissionOptions exceptionTransmission) => _exceptionTransmission = exceptionTransmission;

    public override RpcExceptionHandlingInstructions HandleException
        (ref Exception exception, int procedureId, RpcExecutionStage executionStage)
        => base.HandleException(ref exception, procedureId, executionStage).WithTransmissionOptions(_exceptionTransmission);

    public int Increment(int value) => value + 1;

    public void Store(int value)
    {
        _value = value;
    }

    public void IncrementStored()
    {
        _value++;
    }

    public int GetStored() => _value;

    public void Finish()
    {
        MarkRanToCompletion();
    }

    public string? IncrementString(string? value) => value == null ? null : (int.Parse(value) + 1).ToString();
}