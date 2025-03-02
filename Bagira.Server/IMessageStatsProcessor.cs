namespace Bagira.Server
{
    public interface IMessageStatsProcessor
    {
        void RecordMessage(string message);
    }
}