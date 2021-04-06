using System.Collections.Generic;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public interface IReceivedMessagesProcessingService
    {
        void ProcessReceivedMessageUpdatesFromUsers(List<ReceivedMessageUpdatesFromUsers> receivedMessageUpdatesFromUsers);

        void ProcessReceivedMessageFromUser(ReceivedMessageFromUser receivedMessageFromUser, long updateId);
    }
}