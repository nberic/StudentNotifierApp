using System.Threading.Tasks;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public interface IMessageBroadcastService
    {
        Task<bool> BroadcastNewsNotificationToAllUsers(NewsNotification newsNotification);

        Task<bool> NotifyUserAboutNewsNotification(NewsNotification newsNotification, long UserId);

        Task<bool> SendMessageToUser(string message, long userId, bool isHtml = false);
    }
}
