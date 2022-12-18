﻿namespace MsbRpc.Generator.GenerationHelpers.Names;

public class EndPointNames
{
    public const string BufferSizeParameter = "initialBufferSize";
    public const string DefaultBufferSizeConstant = "DefaultBufferSize";
    public const string EndPointBaseType = "MsbRpc.EndPoints.RpcEndPoint";
    public const string UndefinedProcedureEnum = "MsbRpc.EndPoints.UndefinedProcedure";
    public const string EnterCallingMethod = "EnterCalling";
    public const string ExitCallingMethod = "ExitCalling";

    /// <summary>{ContractNamespace}.Generated.{Contract}{Client/Server}Endpoint.g.cs</summary>
    public string EndPointFile { get; }

    /// <summary>{ContractNamespace}.Generated.I{Contract}{Client/Server}.g.cs</summary>
    public string InterfaceFile { get; }

    /// <summary>{ContractNamespace}.Generated.{Contract}{Client/Server}Procedure.g.cs</summary>
    public string ProcedureFile { get; }

    /// <summary>{ContractNamespace}.Generated.{Contract}{Client/Server}ProcedureExtensions.g.cs</summary>
    public string ProcedureExtensionsFile { get; }

    /// <summary>_{contract}{Client/Server}</summary>
    public string InterfaceField { get; }

    /// <summary>{contract}{Client/Server}</summary>
    public string InterfaceParameter { get; }

    /// <summary>I{Contract}{Client/Server}</summary>
    public string Interface { get; }

    /// <summary>{Contract}{Client/Server}Procedure</summary>
    public string ProcedureEnum { get; }

    /// <summary>{Contract}{Client/Server}ProcedureExtensions</summary>
    public string ProcedureEnumExtensions { get; }

    /// <summary>{Contract}{Client/Server}Endpoint</summary>
    public string EndPointType { get; }

    public EndPointNames(string generatedNamespace, string contractName, string lowerCaseContractName, string serverPostfix)
    {
        InterfaceField = $"_{lowerCaseContractName}{serverPostfix}";
        InterfaceParameter = $"{lowerCaseContractName}{serverPostfix}";

        Interface = $"{GeneralNames.InterfacePrefix}{contractName}{serverPostfix}";
        InterfaceFile = $"{generatedNamespace}.{Interface}{GeneralNames.GeneratedFileEnding}";

        ProcedureEnum = $"{contractName}{serverPostfix}{ContractNames.ProcedurePostfix}";
        ProcedureFile = $"{generatedNamespace}.{ProcedureEnum}{GeneralNames.GeneratedFileEnding}";

        ProcedureEnumExtensions = $"{contractName}{serverPostfix}{ContractNames.ProcedurePostfix}Extensions";
        ProcedureExtensionsFile = $"{generatedNamespace}.{ProcedureEnumExtensions}{GeneralNames.GeneratedFileEnding}";

        EndPointType = $"{contractName}{serverPostfix}Endpoint";
        EndPointFile = $"{generatedNamespace}.{EndPointType}{GeneralNames.GeneratedFileEnding}";
    }
}