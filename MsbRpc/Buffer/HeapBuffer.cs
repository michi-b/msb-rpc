namespace MsbRpc.Buffer;

public class HeapBuffer : IBuffer
{
    public ArraySegment<byte> Borrow(int count) => new(new byte[count]);

    public void Return(ArraySegment<byte> memory)
    {
        //no action needed, as memory is just heap memory
    }
}