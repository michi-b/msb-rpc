using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsbRpsTest.Serialization;

public abstract class SerializationTest<T>
{
    private byte[] _buffer = null!;

    protected abstract int BufferSize { get; }

    [TestInitialize]
    public void Setup()
    {
        _buffer = new byte[BufferSize];
    }

    [TestCleanup]
    public void Cleanup()
    {
        _buffer = null!;
    }

    protected abstract void Write(T value, byte[] buffer);

    protected abstract T Read(byte[] buffer);


    protected void TestPreserves(T value)
    {
        Write(value, _buffer);
        T result = Read(_buffer);
        Assert.AreEqual(value, result);
    }
}