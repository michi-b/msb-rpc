﻿namespace MsbRpc.Messaging;

public enum ReceiveMessageReturnCode
{
    Success = 0,
    ConnectionClosed = 1,
    ConnectionClosedUnexpectedly = 2
}