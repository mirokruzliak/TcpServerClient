using System.Net.Sockets;
using System.Text;

namespace Bagira.Shared
{
    public class CustomTcpClient(TcpClient tcpClient) : IClient
    {
        public async Task SendMessageAsync(string message)
        {
            NetworkStream stream = tcpClient.GetStream();
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(messageBytes);
        }

        public async Task<string> ReadMessageAsync()
        {
            NetworkStream stream = tcpClient.GetStream();

            StringBuilder sb = new();
            byte[] buffer = new byte[1024];
            int bytesRead;

            do
            {
                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            } while (bytesRead == buffer.Length);

            return sb.ToString();
        }

        public void Dispose()
        {
            tcpClient.Close();
        }
    }
}
