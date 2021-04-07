using System.Threading;
using System.Threading.Tasks;

namespace TelegramBot.Services
{
    public interface IRegistryHandlingService
    {
        Task LoadRegisteredIdsToMemoryAsync(string path, CancellationToken cancellationToken);

        Task SaveRegisteredIdsFromMemoryAsync(string path);

        void RegisterId(long id);

        bool ContainsId(long id);

        bool IsEmpty();
    }
}
