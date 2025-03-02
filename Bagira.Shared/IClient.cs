
namespace Bagira.Shared
{
    public interface IClient : IDisposable
    {
        Task<string> ReadMessageAsync();
        Task SendMessageAsync(string message);
    }
}