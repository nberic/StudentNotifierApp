using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TelegramBot.Models
{
    public record ReceivedContentFromEndpoint
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; init; }

        [JsonPropertyName("result")]
        public List<ReceivedMessageUpdatesFromUsers> Result { get; init; }
    }
}
