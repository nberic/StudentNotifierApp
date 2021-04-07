using Microsoft.Extensions.Logging;

namespace TelegramBot.Services
{
    public class ProcessedMessagesRegistryHandlingByFileSystemService : RegistryHandlingByFileSystemService, IProcessedMessagesRegistryHandlingService
    {
        public ProcessedMessagesRegistryHandlingByFileSystemService(ILogger<ProcessedMessagesRegistryHandlingByFileSystemService> logger)
            : base(logger)
        {
        }

        public override void RegisterId(long id)
        {
            lock (_registeredIdsListLock)
            {
                if (IsEmpty())
                {
                    _registeredIdsList.Add(default);
                }

                _registeredIdsList[0] = id;
            }
        }

        public long LastProcessedId
        {
            get
            {
                lock (_registeredIdsListLock)
                {
                    return _registeredIdsList[0];
                }
            }
        }
    }
}
