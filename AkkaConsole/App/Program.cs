using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddEnvironmentVariables();
                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((context, services) => ConfigureServicesHandler(context.Configuration, services))
                .UseConsoleLifetime();
        }

        private static void ConfigureServicesHandler(IConfiguration configuration, IServiceCollection services)
        {
            services.AddHostedService<AkkaSystem>(sp => new AkkaSystem(sp));
        }
    }
}
