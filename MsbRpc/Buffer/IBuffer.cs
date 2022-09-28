namespace MsbRpc.Buffer;

public interface IBuffer
{
    ArraySegment<byte> Borrow(int count);
    void Return(ArraySegment<byte> memory);
}