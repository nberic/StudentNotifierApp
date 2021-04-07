using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using TelegramBot.Models.Configurations;

namespace TelegramBot.Services
{
    public class TelegramBotNewsNotificationsWorkerService : BackgroundService, IHostedService
    {
        private readonly ILogger<TelegramBotNewsNotificationsWorkerService> _logger;

        private readonly IReceivedNewsNotificationsService _receivedNewsNotificationsService;

        private readonly IMessageBroadcastService _newsNotificationsBroadcastService;

        private readonly IOptions<TelegramBotConfiguration> _telegramBotConfiguration;

        public TelegramBotNewsNotificationsWorkerService(   ILogger<TelegramBotNewsNotificationsWorkerService> logger,
                                                            IReceivedNewsNotificationsService receivedNewsNotificationsService,
                                                            IMessageBroadcastService newsNotificationsBroadcastService,
                                                            IOptions<TelegramBotConfiguration> telegramBotConfiguration)
        {
            _logger = logger;
            _receivedNewsNotificationsService = receivedNewsNotificationsService;
            _newsNotificationsBroadcastService = newsNotificationsBroadcastService;
            _telegramBotConfiguration = telegramBotConfiguration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_telegramBotConfiguration.Value.RefreshIntervalInMilliseconds, stoppingToken);

                if (!_receivedNewsNotificationsService.HasUnreadNewsNotifications())
                {
                    _logger.LogInformation("No new message notifications found.");

                    continue;
                }

                var newsNotification = _receivedNewsNotificationsService.GetNewsNotification();

                _logger.LogInformation($"Received news notification: {{id: {newsNotification.Id}, title: {newsNotification.Title} }}");

                var broadcastSuccess = await _newsNotificationsBroadcastService.BroadcastNewsNotificationToAllUsers(newsNotification);

                if (!broadcastSuccess)
                {
                    _logger.LogWarning("Unsuccessful broadcast of received news notification");
                }
                else
                {
                    _logger.LogInformation("Successful broadcaste of received news notifications to all registered users");
                }
            }
        }
    }
}
