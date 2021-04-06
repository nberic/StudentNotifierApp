namespace TelegramBot.Models.Configurations
{
    public class TelegramBotConfiguration
    {
        public int RefreshIntervalInMilliseconds { get; set; }

        public string TelegramBotToken { get; set; }

        public string TelegramOrgUrl { get; set; }

        public string GetUpdatesEndpoint { get; set; }

        public string SendMessageEndpoint { get; set; }

        public RegisteredUsersConfiguration RegisteredUsersConfiguration { get; set; }

        public ProcessedMessagesConfiguration ProcessedMessagesConfiguration { get; set; }
    }
}
