using Bagira.Shared;
using System.Net.Sockets;
using System.Text;

const string serverAddress = "127.0.0.1";
const int port = 1000;

try
{
    using TcpClient client = new TcpClient();
    await client.ConnectAsync(serverAddress, port);
    Console.WriteLine("Connected to the server.");

    var tcpClient = new CustomTcpClient(client);    

    using NetworkStream stream = client.GetStream();

    // Task to continuously read messages from the server
    _ = Task.Run(async () =>
    {
        while (true)
        {
            string receivedMessage = await tcpClient.ReadMessageAsync();
            Console.WriteLine(receivedMessage);
        }
    });

    // Read messages from the command prompt and send to the server
    while (true)
    {
        string messageToSend = Console.ReadLine();
        if (string.IsNullOrEmpty(messageToSend)) 
            continue;
        await tcpClient.SendMessageAsync(messageToSend);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}
