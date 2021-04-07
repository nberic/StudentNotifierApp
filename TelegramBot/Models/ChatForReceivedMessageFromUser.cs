using System.Text.Json.Serialization;

namespace TelegramBot.Models
{
    public record ChatForReceivedMessageFromUser
    {
        [JsonPropertyName("id")]
        public long Id { get; init; }

        [JsonPropertyName("first_name")]
        public string FristName { get; init; }

        [JsonPropertyName("last_name")]
        public string LastName { get; init; }

        [JsonPropertyName("username")]
        public string Username { get; init; }

        [JsonPropertyName("type")]
        public string Type { get; init; }
    }
}
