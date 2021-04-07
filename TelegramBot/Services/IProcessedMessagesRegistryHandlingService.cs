namespace TelegramBot.Services
{
    public interface IProcessedMessagesRegistryHandlingService : IRegistryHandlingService
    {
        long LastProcessedId { get; }
    }
}
