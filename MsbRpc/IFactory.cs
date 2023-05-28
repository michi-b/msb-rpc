namespace MsbRpc;

public interface IFactory<out T>
{
    public T Create();
}