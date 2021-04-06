using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers;

namespace TelegramBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseSerilog((builderContext, loggerConfiguration) =>
                    {
                        loggerConfiguration.ReadFrom.
                            Configuration(builderContext.Configuration).
                            Enrich.With(new ThreadIdEnricher()).
                            Enrich.With(new ProcessIdEnricher());
                    });
                });
    }
}
