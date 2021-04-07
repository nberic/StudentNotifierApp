using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TelegramBot.Models;

namespace TelegramBot.Utilities
{
    public class ResponseDeserializeService : IResponseDeserializeService
    {
        private readonly Regex _okRegex = new("{\\s*\"ok\"\\s*:\\s*true\\s*.*}", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public async Task<bool> IsOkay(HttpResponseMessage httpResponseMessage)
        {
            var receivedPayload = await httpResponseMessage?.Content?.ReadAsStringAsync();

            return _okRegex.IsMatch(receivedPayload);
        }

        public async Task<bool> IsOkay(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
        {
            var receivedPayload = await httpResponseMessage?.Content?.ReadAsStringAsync(cancellationToken);

            return _okRegex.IsMatch(receivedPayload);
        }

        public async Task<ReceivedContentFromEndpoint> DeserializeHttpResponseAsync(HttpResponseMessage httpResponseMessage)
        {
            var receivedPayload = await httpResponseMessage?.Content?.ReadAsStringAsync();

            return JsonSerializer.
                    Deserialize<ReceivedContentFromEndpoint>(receivedPayload);
        }

        public async Task<ReceivedContentFromEndpoint> DeserializeHttpResponseAsync(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
        {
            var receivedPayload = await httpResponseMessage?.Content?.ReadAsStringAsync(cancellationToken);

            return JsonSerializer.
                    Deserialize<ReceivedContentFromEndpoint>(receivedPayload);
        }
    }
}
