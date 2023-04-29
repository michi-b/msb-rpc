using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;
using MsbRpc.Test.Generator.Base;

namespace MsbRpc.Test.Generator;

[TestClass]
public class DateTimeEchoGenerationTest : ContractGenerationTest<DateTimeEchoGenerationTest, ContractGenerator>
{
    private const string Code = @"[ConstantSizeSerializer(typeof(DateTime))]
public static class DateTimeSerializer
{
    [SerializedSize] public const int Size = PrimitiveSerializer.LongSize;

    [SerializationMethod]
    public static void Write(BufferWriter writer, DateTime value)
    {
        writer.Write(value.Ticks);
    }

    [DeserializationMethod]
    public static DateTime Read(BufferReader reader) => new(reader.ReadLong());
}

[RpcContract(RpcContractType.ClientToServer)]
public interface IDateTimeEcho : IRpcContract
{
    System.DateTime GetDateTime(System.DateTime myDateTime);
}";

    private const string Namespace = "MsbRpc.Test.Generator.Echo.Tests";

    public DateTimeEchoGenerationTest() : base(Code, Namespace) { }

    [TestMethod]
    public async Task GeneratorHasResult()
    {
        await TestGeneratorHasResult();
    }

    [TestMethod]
    public async Task GeneratorThrowsNoException()
    {
        await TestGeneratorThrowsNoException();
    }

    [TestMethod]
    public async Task GeneratorReportsNoDiagnostics()
    {
        await TestGeneratorReportsNoDiagnostics();
    }

    [TestMethod]
    public async Task FinalCompilationReportsNoDiagnostics()
    {
        await TestFinalCompilationReportsNoDiagnostics();
    }

    [TestMethod]
    public async Task GeneratesAnyTrees()
    {
        await TestGeneratesAnyTrees();
    }
}