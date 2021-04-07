using System.Text.Json.Serialization;

namespace TelegramBot.Models
{
    public record EntityForReceivedMessageFromUser
    {
        [JsonPropertyName("offset")]
        public int Offset { get; init; }

        [JsonPropertyName("length")]
        public int Length { get; init; }

        [JsonPropertyName("type")]
        public string Type { get; init; }
    }
}
