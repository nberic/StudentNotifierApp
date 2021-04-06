using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TelegramBot.Models
{
    public record ReceivedMessageFromUser
    {
        [JsonPropertyName("message_id")]
        public long MessageId { get; init; }

        [JsonPropertyName("from")]
        public FromForReceivedMessageFromUser From { get; init; }

        [JsonPropertyName("chat")]
        public ChatForReceivedMessageFromUser Chat { get; init; }

        [JsonPropertyName("date")]
        public long Date { get; init; }

        [JsonPropertyName("text")]
        public string Text { get; init; }

        [JsonPropertyName("entities")]
        public List<EntityForReceivedMessageFromUser> Entities { get; init; }
    }
}
