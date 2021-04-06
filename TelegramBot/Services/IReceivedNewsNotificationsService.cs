using TelegramBot.Models;

namespace TelegramBot.Services
{
    public interface IReceivedNewsNotificationsService
    {
        void AddNewsNotification(NewsNotification newsNotification);

        NewsNotification GetNewsNotification();

        bool HasUnreadNewsNotifications();
    }
}
