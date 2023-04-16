using System.Collections.Immutable;
using System.Net;
using Microsoft.Extensions.Logging;
using Misbat.CodeAnalysis.Test.CodeTest;
using Misbat.CodeAnalysis.Test.Utility;
using MsbRpc.Contracts;
using MsbRpc.Generator;
using MsbRpc.Generator.Attributes;

namespace MsbRpc.Test.Generator;

public static class CodeTestConfigurationUtility
{
    public static CodeTestConfiguration GetContractGeneratorCodeTestConfiguration<TTest>()
        where TTest : Test
        => new CodeTestConfiguration
        (
            ImmutableArray.Create
            (
                MetadataReferenceUtility.MsCoreLib,
                MetadataReferenceUtility.SystemRuntime,
                MetadataReferenceUtility.NetStandard,
                MetadataReferenceUtility.FromType<RpcContractAttribute>(), //MsbRpc.Generator.Attributes
                MetadataReferenceUtility.FromType<IRpcContract>(), //MsbRpc
                MetadataReferenceUtility.FromType<ILoggerFactory>(), //Microsoft.Extensions.Logging
                MetadataReferenceUtility.FromType<IPAddress>(), //System.Net.Primitives
                MetadataReferenceUtility.TransitivelyReferenced(typeof(TTest), "System.Threading.Tasks.Extensions"),
                MetadataReferenceUtility.TransitivelyReferenced(typeof(TTest), "System.Threading.Thread")
            )
        ).WithAdditionalGenerators(new ContractGenerator());
}