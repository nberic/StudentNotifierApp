using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public class ReceivedNewsNotificationsService : IReceivedNewsNotificationsService
    {
        private readonly ILogger<ReceivedNewsNotificationsService> _logger;

        private readonly Queue<NewsNotification> _newsNotifications = new();

        private readonly object _newsNotificationsLock = new();

        public ReceivedNewsNotificationsService(ILogger<ReceivedNewsNotificationsService> logger)
        {
            _logger = logger;
        }

        public void AddNewsNotification(NewsNotification newsNotification)
        {
            lock (_newsNotificationsLock)
            {
                _newsNotifications.Enqueue(newsNotification);
            }
        }

        public NewsNotification GetNewsNotification()
        {
            NewsNotification newsNotification = null;

            lock (_newsNotificationsLock)
            {
                if (_newsNotifications.Count > 0)
                {
                    newsNotification = _newsNotifications.Dequeue();
                }
            }

            return newsNotification;
        }

        public bool HasUnreadNewsNotifications()
        {
            lock (_newsNotificationsLock)
            {
                return _newsNotifications.Count > 0;
            }
        }
    }
}
