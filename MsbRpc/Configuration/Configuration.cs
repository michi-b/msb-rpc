﻿using Microsoft.Extensions.Logging;

namespace MsbRpc.Configuration;

public class Configuration
{
    public ILoggerFactory? LoggerFactory { get; set; } = null;
}