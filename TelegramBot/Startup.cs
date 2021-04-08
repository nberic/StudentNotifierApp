using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TelegramBot.Models.Configurations;
using TelegramBot.Services;
using TelegramBot.Utilities;

namespace TelegramBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TelegramBot", Version = "v1" });
            });

            services.Configure<TelegramBotConfiguration>(Configuration.GetSection(nameof(TelegramBotConfiguration)));
            services.Configure<UserMessageReplyConfiguration>(Configuration.GetSection(nameof(UserMessageReplyConfiguration)));

            services.AddHttpClient();

            services.AddHostedService<TelegramBotNewsNotificationsWorkerService>();
            services.AddHostedService<TelegramBotReceivedMessagesWorkerService>();

            services.AddSingleton<IReceivedNewsNotificationsService, ReceivedNewsNotificationsService>();
            services.AddTransient<IMessageBroadcastService, MessageBroadcastService>();
            services.AddTransient<IResponseDeserializeService, ResponseDeserializeService>();
            services.AddSingleton<IReceivedMessagesProcessingService, ReceivedMessagesProcessingService>();
            services.AddSingleton<IRegisteredUsersHandlingService, RegisteredUsersHandlingByFileSystemService>();
            services.AddSingleton<IProcessedMessagesRegistryHandlingService, ProcessedMessagesRegistryHandlingByFileSystemService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TelegramBot v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
