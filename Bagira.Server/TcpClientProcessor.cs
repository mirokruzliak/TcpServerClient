using System.Net.Sockets;
using System.Net;
using Bagira.Shared;

namespace Bagira.Server
{
    public class TcpClientProcessor(IClientManager handler, ConnectionsSettings connectionsSettings) : IClientProcessor
    {
        public async Task AcceptClientsAsync()
        {
            var ipEndPoint = new IPEndPoint(connectionsSettings.IPAddress, connectionsSettings.Port);
            TcpListener listener = new(ipEndPoint);

            try
            {
                listener.Start();

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();

                    Thread t = new Thread(() => handler.HandleClient(new CustomTcpClient(client)));
                    t.Start();
                }
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}
