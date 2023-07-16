#region

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MsbRpc.Generator;
using MsbRpc.Test.Generator.Base;

#endregion

namespace MsbRpc.Test.Generator.CodeGeneration;

[TestClass]
[TestCategory(TestCategories.Contract)]
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

[RpcContract]
public interface IDateTimeEcho : IRpcContract
{
    System.DateTime GetDateTime(System.DateTime myDateTime);
}";

    private const string Namespace = nameof(DateTimeEchoGenerationTest);

    private const string ContractName = "DateTimeEcho";

    public DateTimeEchoGenerationTest() : base(Code, Namespace, ContractName) { }

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

    [TestMethod]
    public async Task GeneratesServerEndPoint()
    {
        await TestGeneratesServerEndPoint();
    }

    [TestMethod]
    public async Task GeneratesClientEndPoint()
    {
        await TestGeneratesClientEndPoint();
    }

    [TestMethod]
    public async Task GeneratesClientEndPointConfigurationBuilder()
    {
        await TestGeneratesClientEndPointConfigurationBuilder();
    }
}