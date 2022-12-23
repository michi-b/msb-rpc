using MsbRpc.Generator.Info;

namespace MsbRpc.Generator.GenerationHelpers.Names;

public class EndPointNames
{
    public static class Types
    {
        public const string EndPointBaseType = "MsbRpc.EndPoints.RpcEndPoint";
        public const string UndefinedProcedureEnum = "MsbRpc.EndPoints.UndefinedProcedure";
    }
    
    public static class Methods
    {
        public const string EnterCalling = "EnterCalling";
        public const string ExitCalling = "ExitCalling";
        public const string GetRequestWriter = "GetRequestWriter";
        public const string GetResponseWriter = "GetResponseWriter";
    }
    
    public static class Parameters
    {
        public const string BufferSize = "initialBufferSize";
        public const string ArgumentsBufferReader = "arguments";
    }
    
    public static class Fields
    {
        public const string DefaultBufferSizeConstant = "DefaultBufferSize";
    }

    public string EndPointFile { get; }
    public string InterfaceFile { get; }
    public string InboundProcedureEnumFile { get; }
    public string InboundProcedureEnumExtensionsFile { get; }
    public string InterfaceField { get; }
    public string InterfaceParameter { get; }
    public string InterfaceType { get; }
    public string InboundProcedureEnumType { get; }
    public string InboundProcedureEnumParameter { get; }
    public string InboundProcedureEnumExtensionsType { get; }
    public string EndPointType { get; }
    
    public EndPointNames(ref ContractInfo contractInfo, ContractNames contractNames, EndPointId endPointType)
    {
        string endPointTypeName = endPointType.GetName();
        string lowerCaseEndPointTypeName = endPointType.GetLowerCaseName();
        string contractName = contractNames.ContractName;
        string lowerCaseContractName = contractNames.LowerCaseContractName;
        string generatedNamespace = contractNames.GeneratedNamespace;

        bool hasInboundProcedures = contractInfo[endPointType].HasInboundProcedures;
        
        InterfaceType = $"{GeneralNames.InterfacePrefix}{contractName}{endPointTypeName}";
        InterfaceFile = $"{generatedNamespace}.{InterfaceType}{GeneralNames.GeneratedFileEnding}";
        InterfaceParameter = $"{lowerCaseContractName}{endPointTypeName}";
        InterfaceField = $"_{lowerCaseContractName}{endPointTypeName}";

        EndPointType = $"{contractName}{endPointTypeName}EndPoint";
        EndPointFile = $"{generatedNamespace}.{EndPointType}{GeneralNames.GeneratedFileEnding}";
        
        if (hasInboundProcedures)
        {
            InboundProcedureEnumType = $"{contractName}{endPointTypeName}{ContractNames.ProcedurePostfix}";
            InboundProcedureEnumFile = $"{generatedNamespace}.{InboundProcedureEnumType}{GeneralNames.GeneratedFileEnding}";
            InboundProcedureEnumParameter = $"{lowerCaseEndPointTypeName}{ContractNames.ProcedurePostfix}";
            InboundProcedureEnumExtensionsType = $"{contractName}{endPointTypeName}{ContractNames.ProcedurePostfix}Extensions";
            InboundProcedureEnumExtensionsFile = $"{generatedNamespace}.{InboundProcedureEnumExtensionsType}{GeneralNames.GeneratedFileEnding}";
        }
        else
        {
            InboundProcedureEnumType = Types.UndefinedProcedureEnum;
            InboundProcedureEnumFile = string.Empty;
            InboundProcedureEnumParameter = string.Empty;
            InboundProcedureEnumExtensionsType = string.Empty;
            InboundProcedureEnumExtensionsFile = string.Empty;
        }
    }
}