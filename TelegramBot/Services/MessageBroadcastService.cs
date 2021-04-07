using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TelegramBot.Models;
using TelegramBot.Models.Configurations;
using TelegramBot.Utilities;

namespace TelegramBot.Services
{
    public class MessageBroadcastService : IMessageBroadcastService
    {
        private readonly ILogger<MessageBroadcastService> _logger;

        private readonly IOptions<TelegramBotConfiguration> _telegramBotConfiguration;

        private readonly IOptions<UserMessageReplyConfiguration> _userMessageReplyConfiguration;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IResponseDeserializeService _responseDeserializeService;

        private readonly IRegisteredUsersHandlingService _registeredUsersHandlingService;

        public MessageBroadcastService(ILogger<MessageBroadcastService> logger,
                                        IOptions<TelegramBotConfiguration> telegramBotConfiguration,
                                        IOptions<UserMessageReplyConfiguration> userMessageReplyConfiguration,
                                        IHttpClientFactory httpClientFactory,
                                        IResponseDeserializeService responseDeserializeService,
                                        IRegisteredUsersHandlingService registeredUsersHandlingService)
        {
            _logger = logger;
            _telegramBotConfiguration = telegramBotConfiguration;
            _userMessageReplyConfiguration = userMessageReplyConfiguration;
            _httpClientFactory = httpClientFactory;
            _responseDeserializeService = responseDeserializeService;
            _registeredUsersHandlingService = registeredUsersHandlingService;
        }

        public async Task<bool> BroadcastNewsNotificationToAllUsers(NewsNotification newsNotification)
        {
            var newsNotificationBroadcastSuccess = true;

            foreach (var userId in _registeredUsersHandlingService.AllRegisteredIds)
            {
                var newsNotificationBroadcastSuccessForUser = await NotifyUserAboutNewsNotification(newsNotification, userId);

                if (!newsNotificationBroadcastSuccessForUser)
                {
                    newsNotificationBroadcastSuccess = false;
                }
            }

            return newsNotificationBroadcastSuccess;
        }

        public async Task<bool> NotifyUserAboutNewsNotification(NewsNotification newsNotification, long userId)
        {
            var message = $"<b>{newsNotification.Title}</b>\n {_userMessageReplyConfiguration.Value.NewsNotificationText} {newsNotification.Url}";

            var isMessageSent = await SendMessageToUser(message, userId, true);

            return isMessageSent;
        }

        public async Task<bool> SendMessageToUser(string message, long userId, bool isHtml = false)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var newsNotificationData = new Dictionary<string, string>()
            {
                { "chat_id", userId.ToString() },
                { "text", message }
            };

            if (isHtml)
            {
                newsNotificationData["parse_mode"] = "HTML";
            }

            var payload = new FormUrlEncodedContent(newsNotificationData);

            var endpointUrl = $"https://" +
                    $"{_telegramBotConfiguration.Value.TelegramOrgUrl}" +
                    $"/bot{_telegramBotConfiguration.Value.TelegramBotToken}" +
                    $"/{_telegramBotConfiguration.Value.SendMessageEndpoint}";

            var requestResponse = await httpClient.PostAsync(endpointUrl, payload);

            var isOkay = await _responseDeserializeService.IsOkay(requestResponse);

            if (!requestResponse.IsSuccessStatusCode || !isOkay)
            {
                _logger.LogWarning($"Unable to send message to user {userId}: \"{message}\"");
                return false;
            }

            return true;
        }
    }
}
