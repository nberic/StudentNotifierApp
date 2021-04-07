namespace TelegramBot.Models.Configurations
{
    public class UserMessageReplyConfiguration
    {
        public string NewsNotificationText { get; set; }

        public string StartCommandReplyText { get; set; }

        public string StopCommandReplyText { get; set; }

        public string UnsupportedCommandReplyText { get; set; }

        public string UnregisteredUserSentMessageText { get; set; }
    }
}
