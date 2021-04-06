using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using TelegramBot.Models;
using TelegramBot.Models.Configurations;

namespace TelegramBot.Services
{
    public class ReceivedMessagesProcessingService : IReceivedMessagesProcessingService
    {
        private readonly ILogger<ReceivedMessagesProcessingService> _logger;

        private readonly IMessageBroadcastService _messageBroadcastService;

        private readonly IRegisteredUsersHandlingService _registeredUsersHandlingService;

        private readonly IProcessedMessagesRegistryHandlingService _processedMessagesRegistryHandlingService;

        private readonly IOptions<UserMessageReplyConfiguration> _userMessageReplyConfiguration;

        public ReceivedMessagesProcessingService(   ILogger<ReceivedMessagesProcessingService> logger,
                                                    IMessageBroadcastService messageBroadcastService,
                                                    IRegisteredUsersHandlingService registeredUsersHandlingService,
                                                    IProcessedMessagesRegistryHandlingService processedMessagesRegistryHandlingService,
                                                    IOptions<UserMessageReplyConfiguration> userMessageReplyConfiguration)
        {
            _logger = logger;
            _messageBroadcastService = messageBroadcastService;
            _registeredUsersHandlingService = registeredUsersHandlingService;
            _processedMessagesRegistryHandlingService = processedMessagesRegistryHandlingService;
            _userMessageReplyConfiguration = userMessageReplyConfiguration;
        }

        public void ProcessReceivedMessageUpdatesFromUsers(List<ReceivedMessageUpdatesFromUsers> receivedMessageUpdatesFromUsers)
        {
            foreach (var receivedMessageFromUser in receivedMessageUpdatesFromUsers)
            {
                ProcessReceivedMessageFromUser(receivedMessageFromUser.Message, receivedMessageFromUser.UpdateId);
            }
        }

        public void ProcessReceivedMessageFromUser(ReceivedMessageFromUser receivedMessageFromUser, long updateId)
        {
            switch (receivedMessageFromUser.Text)
            {
                case "/start":
                    ProcessReceivedStartCommand(receivedMessageFromUser);
                    break;

                case "/stop":
                    ProcessReceivedStopCommand(receivedMessageFromUser);
                    break;

                default:
                    ProcessUnsupportedCommand(receivedMessageFromUser);
                    break;
            }

            _processedMessagesRegistryHandlingService.RegisterId(updateId);
        }

        private void ProcessReceivedStartCommand(ReceivedMessageFromUser receivedMessageFromUser)
        {
            _registeredUsersHandlingService.RegisterId(receivedMessageFromUser.From.Id);

            _messageBroadcastService.SendMessageToUser( _userMessageReplyConfiguration.Value.StartCommandReplyText,
                                                        receivedMessageFromUser.From.Id);
        }

        private void ProcessReceivedStopCommand(ReceivedMessageFromUser receivedMessageFromUser)
        {
            _registeredUsersHandlingService.UnregisterId(receivedMessageFromUser.From.Id);

            _messageBroadcastService.SendMessageToUser( _userMessageReplyConfiguration.Value.StopCommandReplyText,
                                                        receivedMessageFromUser.From.Id);
        }

        private void ProcessUnsupportedCommand(ReceivedMessageFromUser receivedMessageFromUser)
        {
            if (_registeredUsersHandlingService.ContainsId(receivedMessageFromUser.From.Id))
            {

                _messageBroadcastService.SendMessageToUser( _userMessageReplyConfiguration.Value.UnsupportedCommandReplyText,
                                                            receivedMessageFromUser.From.Id);
            }
            else
            {
                _messageBroadcastService.SendMessageToUser( _userMessageReplyConfiguration.Value.UnregisteredUserSentMessageText,
                                                            receivedMessageFromUser.From.Id);
            }
        }
    }
}
