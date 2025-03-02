using System.Net;
using Bagira.Server;

var clientProcessor = new TcpClientProcessor(
    new CustomClientManager(new LetterCounter()), 
    new ConnectionSettings
    {
        IPAddress = IPAddress.Any,
        Port = 1000
    }
);

await clientProcessor.AcceptClientsAsync();

