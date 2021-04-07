using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace TelegramBot.Services
{
    public class RegisteredUsersHandlingByFileSystemService : RegistryHandlingByFileSystemService, IRegisteredUsersHandlingService
    {
        public RegisteredUsersHandlingByFileSystemService(ILogger<RegisteredUsersHandlingByFileSystemService> logger)
            : base(logger)
        {
        }

        public IEnumerable<long> AllRegisteredIds
        {
            get
            {
                lock (_registeredIdsListLock)
                {
                    return _registeredIdsList;
                }
            }
        }

        public override void RegisterId(long id)
        {
            
            lock (_registeredIdsListLock)
            {
                _registeredIdsList.Add(id);
            }
        }

        public void UnregisterId(long id)
        {
            lock (_registeredIdsListLock)
            {
                _registeredIdsList.Remove(id);
            }
        }
    }
}
