namespace MsbRps.Interfaces
{
    public interface IRpsSerializable
    {
        Task Deserialize(Stream stream, byte[] buffer, int offset, CancellationToken cancellationToken);

        Task Serialize(Stream stream, byte[] buffer, int offset, CancellationToken cancellationToken);
    }
}