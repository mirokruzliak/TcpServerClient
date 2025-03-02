using Bagira.Shared;

namespace Bagira.Server
{
    public interface IClientManager
    {
        void HandleClient(IClient client);
    }
}