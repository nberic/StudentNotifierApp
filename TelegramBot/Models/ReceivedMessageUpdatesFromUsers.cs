using System.Text.Json.Serialization;

namespace TelegramBot.Models
{
    public record ReceivedMessageUpdatesFromUsers
    {
        [JsonPropertyName("update_id")]
        public long UpdateId { get; init; }

        [JsonPropertyName("message")]
        public ReceivedMessageFromUser Message { get; init; }
    }
}
