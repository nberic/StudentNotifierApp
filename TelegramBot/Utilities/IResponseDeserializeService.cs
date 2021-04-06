using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TelegramBot.Models;

namespace TelegramBot.Utilities
{
    public interface IResponseDeserializeService
    {
        Task<ReceivedContentFromEndpoint> DeserializeHttpResponseAsync(HttpResponseMessage httpResponseMessage);

        Task<ReceivedContentFromEndpoint> DeserializeHttpResponseAsync(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken);

        Task<bool> IsOkay(HttpResponseMessage httpResponseMessage);

        Task<bool> IsOkay(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken);
    }
}