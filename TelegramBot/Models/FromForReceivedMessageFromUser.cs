﻿using System.Text.Json.Serialization;

namespace TelegramBot.Models
{
    public record FromForReceivedMessageFromUser
    {
        [JsonPropertyName("id")]
        public long Id { get; init; }

        [JsonPropertyName("is_bot")]
        public bool IsBot{ get; init; }

        [JsonPropertyName("first_name")]
        public string FirstName { get; init; }

        [JsonPropertyName("last_name")]
        public string LastName { get; init; }

        [JsonPropertyName("username")]
        public string Username { get; init; }

        [JsonPropertyName("language_code")]
        public string LanguageCode { get; init; }
    }
}
