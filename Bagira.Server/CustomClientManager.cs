using Bagira.Shared;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Bagira.Server
{
    public class CustomClientManager(IMessageStatsProcessor messageStatsProcessor) : IClientManager
    {
        private readonly ConcurrentDictionary<string, IClient> _clients = new();

        public async void HandleClient(IClient client)
        {
            string name = string.Empty;

            try
            {
                await client.SendMessageAsync("Please enter name");
                name = await client.ReadMessageAsync();
                _clients.TryAdd(name, client);

                while (true)
                {
                    var clientMessage = await client.ReadMessageAsync();
                    Console.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss}, {name} - {clientMessage}");

                    var match = Regex.Match(clientMessage, @"To:\s*(?<ClientName>\w+)\s*-\s*(?<MessageContent>.+)");
                    if (match.Success)
                    {
                        string clientName = match.Groups["ClientName"].Value;
                        string messageContent = match.Groups["MessageContent"].Value;

                        if (_clients.TryGetValue(clientName, out var clientToSendMessage))
                        {
                            await clientToSendMessage.SendMessageAsync($"{name} - {messageContent}");
                        }
                    }
                    else
                    {
                        var messageTasks = _clients.Where(c => !c.Key.Equals(name)).Select(c => c.Value.SendMessageAsync(clientMessage));
                        await Task.WhenAll(messageTasks);
                    }

                    messageStatsProcessor.RecordMessage(clientMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                if (!string.IsNullOrEmpty(name))
                {
                    _clients.Remove(name, out var removedClient);
                    removedClient?.Dispose();
                }

                client.Dispose();
            }
        }
    }
}
