using MsbRps.Interfaces;

namespace MsbRpsConsoleTest
{
    public partial class Message : IRpsSerializable 
          
    {
        public Task Deserialize(Stream stream, byte[] buffer, int offset, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Serialize(Stream stream, byte[] buffer, int offset, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
