namespace TelegramBot.Models
{
    public record NewsNotification
    {
        public int Id { get; init; }

        public string Title { get; init; }

        public string Url { get; init; }
    }
}
