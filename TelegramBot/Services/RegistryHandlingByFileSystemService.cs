using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TelegramBot.Services
{
    public abstract class RegistryHandlingByFileSystemService : IRegistryHandlingService
    {
        private readonly ILogger<RegistryHandlingByFileSystemService> _logger;

        protected readonly object _registeredIdsListLock = new();

        protected List<long> _registeredIdsList;

        public RegistryHandlingByFileSystemService(ILogger<RegistryHandlingByFileSystemService> logger)
        {
            _logger = logger;
        }

        public abstract void RegisterId(long id);

        public virtual bool ContainsId(long id)
        {
            lock (_registeredIdsListLock)
            {
                return _registeredIdsList.Contains(id);
            }
        }

        public virtual bool IsEmpty()
        {
            lock (_registeredIdsListLock)
            {
                return _registeredIdsList.Count == 0;
            }
        }

        public async virtual Task LoadRegisteredIdsToMemoryAsync(string path, CancellationToken cancellationToken)
        {
            
            if (!File.Exists(path))
            {
                CreateFileWithSupportForDirectoryCreation(path);

                _logger.LogInformation($"Created new list of registered IDs for {this.GetType().Name}");

                return;
            }

            var registeredUsersFileContent = await File.ReadAllBytesAsync(path, cancellationToken);

            var registeredUsersDeserialized = await JsonSerializer.
                DeserializeAsync<List<long>>(new MemoryStream(registeredUsersFileContent), cancellationToken: cancellationToken);

            lock (_registeredIdsListLock)
            {
                if (registeredUsersDeserialized is null)
                {
                    _registeredIdsList = new List<long>();
                }

                _registeredIdsList = registeredUsersDeserialized;
            }

            _logger.LogInformation($"Successfully loaded registered IDs from File System for for {this.GetType().Name}");
        }

        public async virtual Task SaveRegisteredIdsFromMemoryAsync(string path)
        {
            string registeredUsersSerialized = null;

            lock (_registeredIdsListLock)
            {
                registeredUsersSerialized = JsonSerializer.Serialize(_registeredIdsList);
            }

            await File.WriteAllTextAsync(path, registeredUsersSerialized);

            _logger.LogInformation($"Successfully saved registered IDs for {this.GetType().Name}.");
        }

        private void CreateFileWithSupportForDirectoryCreation(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.Create(path).Close();

            lock (_registeredIdsListLock)
            {
                _registeredIdsList = new List<long>();
            }
        }
    }
}
