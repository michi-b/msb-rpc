using System;
using Incrementer;
using MsbRpc.Contracts;
using MsbRpc.Exceptions;

namespace MsbRpc.Test.Generator.Incrementer;

internal class Incrementer : IIncrementer
{
    private readonly RpcExceptionTransmissionOptions _exceptionTransmission;
    private int _value;

    public bool RanToCompletion { get; private set; }

    public Incrementer(RpcExceptionTransmissionOptions exceptionTransmission) => _exceptionTransmission = exceptionTransmission;

    public RpcExceptionHandlingInstructions HandleException
        (ref Exception exception, int procedureId, RpcExecutionStage executionStage)
        => RpcExceptionHandlingInstructions.Default.WithTransmissionOptions(_exceptionTransmission);

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
        RanToCompletion = true;
    }

    public string? IncrementString(string? value) => value == null ? null : (int.Parse(value) + 1).ToString();

    public void Dispose() { }
}