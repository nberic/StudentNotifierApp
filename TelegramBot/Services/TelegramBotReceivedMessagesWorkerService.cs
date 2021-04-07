using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TelegramBot.Models.Configurations;
using TelegramBot.Utilities;

namespace TelegramBot.Services
{
    public class TelegramBotReceivedMessagesWorkerService : BackgroundService, IHostedService
    {
        private readonly ILogger<TelegramBotReceivedMessagesWorkerService> _logger;

        private readonly IOptions<TelegramBotConfiguration> _telegramBotConfiguration;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IResponseDeserializeService _responseDeserializeService;

        private readonly IReceivedMessagesProcessingService _receivedMessagesProcessingService;

        private readonly IRegisteredUsersHandlingService _registeredUsersHandlingService;

        private readonly IProcessedMessagesRegistryHandlingService _processedMessagesHandlingService;

        public TelegramBotReceivedMessagesWorkerService(ILogger<TelegramBotReceivedMessagesWorkerService> logger,
                                                        IOptions<TelegramBotConfiguration> telegramBotConfiguration,
                                                        IHttpClientFactory httpClientFactory,
                                                        IResponseDeserializeService responseDeserializeService,
                                                        IReceivedMessagesProcessingService receivedMessagesProcessingService,
                                                        IRegisteredUsersHandlingService registeredUsersHandlingService,
                                                        IProcessedMessagesRegistryHandlingService processedMessagesHandlingService)
        {
            _logger = logger;
            _telegramBotConfiguration = telegramBotConfiguration;
            _httpClientFactory = httpClientFactory;
            _responseDeserializeService = responseDeserializeService;
            _receivedMessagesProcessingService = receivedMessagesProcessingService;
            _registeredUsersHandlingService = registeredUsersHandlingService;
            _processedMessagesHandlingService = processedMessagesHandlingService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await LoadRegistrySettingsAsync(stoppingToken);

            var httpClient = _httpClientFactory.CreateClient();

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_telegramBotConfiguration.Value.RefreshIntervalInMilliseconds, stoppingToken);

                var endpointUrl = $"https://" +
                    $"{_telegramBotConfiguration.Value.TelegramOrgUrl}" +
                    $"/bot{_telegramBotConfiguration.Value.TelegramBotToken}" +
                    $"/{_telegramBotConfiguration.Value.GetUpdatesEndpoint}";

                if (!_processedMessagesHandlingService.IsEmpty())
                {
                    endpointUrl += $"?offset={_processedMessagesHandlingService.LastProcessedId + 1}";
                }

                var requestResponse = await httpClient.GetAsync(endpointUrl, stoppingToken);


                var receivedContentFromEndpoint = await _responseDeserializeService.DeserializeHttpResponseAsync(requestResponse, stoppingToken);

                if (!requestResponse.IsSuccessStatusCode || !(receivedContentFromEndpoint?.Ok ?? false))
                {
                    _logger.LogWarning($"Unable to get updates from telegram API at {endpointUrl}");
                    continue;
                }

                await Task.Run(() =>
                {
                    _receivedMessagesProcessingService.ProcessReceivedMessageUpdatesFromUsers(receivedContentFromEndpoint.Result);
                }, stoppingToken);
            }
        }

        private async Task LoadRegistrySettingsAsync(CancellationToken stoppingToken)
        {
            await LoadRegisteredUsersConfigurationAsync(stoppingToken);

            await LoadProcessedMessagesConfigurationAsync(stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await SaveRegisteredUsersConfigurationAsync();
            await SaveProcessedMessagesConfigurationAsync();

            await base.StopAsync(cancellationToken);
        }

        private async Task LoadRegisteredUsersConfigurationAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _registeredUsersHandlingService.
                    LoadRegisteredIdsToMemoryAsync(_telegramBotConfiguration.Value.RegisteredUsersConfiguration.Path, cancellationToken);

            }
            catch (IOException ex)
            {
                _logger.LogError("Unable to load configuration of registered users at" +
                    $"{_telegramBotConfiguration.Value.RegisteredUsersConfiguration}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unknown error has occurred.", ex);
            }
        }

        private async Task LoadProcessedMessagesConfigurationAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _processedMessagesHandlingService.
                    LoadRegisteredIdsToMemoryAsync(_telegramBotConfiguration.Value.ProcessedMessagesConfiguration.Path, cancellationToken);
            }
            catch (IOException ex)
            {
                _logger.LogError("Unable to load configuration of previously processed messages at" +
                    $"{_telegramBotConfiguration.Value.ProcessedMessagesConfiguration.Path}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unknown error has occurred.", ex);
            }
        }

        private async Task SaveRegisteredUsersConfigurationAsync()
        {
            try
            {
                await _registeredUsersHandlingService.
                    SaveRegisteredIdsFromMemoryAsync(_telegramBotConfiguration.Value.RegisteredUsersConfiguration.Path);

            }
            catch (IOException ex)
            {
                _logger.LogError("Unable to save configuration of registered users at" +
                    $"{_telegramBotConfiguration.Value.RegisteredUsersConfiguration}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unknown error has occurred.", ex);
            }
        }

        private async Task SaveProcessedMessagesConfigurationAsync()
        {
            try
            {
                await _processedMessagesHandlingService.
                    SaveRegisteredIdsFromMemoryAsync(_telegramBotConfiguration.Value.ProcessedMessagesConfiguration.Path);
            }
            catch (IOException ex)
            {
                _logger.LogError("Unable to save configuration of previously processed messages at" +
                    $"{_telegramBotConfiguration.Value.ProcessedMessagesConfiguration.Path}.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unknown error has occurred.", ex);
            }
        }
    }
}
