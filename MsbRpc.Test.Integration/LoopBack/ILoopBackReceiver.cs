﻿using MsbRpc.Contracts;
using MsbRpc.Generator.Attributes;

namespace MsbRpc.Test.Integration.LoopBack;

[RpcContract(RpcContractType.ClientToServer)]
public interface ILoopBackReceiver : IRpcContract
{
    void InitiateLoopBack(int value);
}