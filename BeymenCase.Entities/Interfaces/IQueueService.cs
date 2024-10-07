namespace BeymenCase.Core.Interfaces
{
    public interface IQueueService
    {
        void SendMessage(string message);
    }
}
