using System.Collections.Generic;

namespace TelegramBot.Services
{
    public interface IRegisteredUsersHandlingService : IRegistryHandlingService
    {
        IEnumerable<long> AllRegisteredIds { get; }

        void UnregisterId(long id);
    }
}
