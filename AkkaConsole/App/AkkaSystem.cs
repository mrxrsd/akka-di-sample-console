using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App
{
    public class AkkaSystem : IHostedService
    {
        private readonly IServiceProvider _services;
        private ActorSystem _actorSystem;

        public AkkaSystem(IServiceProvider services)
        {
            _services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var hocon = ConfigurationLoader.Load();
            var bootstrap = BootstrapSetup.Create().WithConfig(hocon);
            var di = DependencyResolverSetup.Create(_services);
            var actorSystemSetup = bootstrap.And(di);
            _actorSystem = ActorSystem.Create("helloSystem", actorSystemSetup);

             _actorSystem.ActorOf(DependencyResolver.For(_actorSystem).Props<HelloActor>());
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }

    public static class ConfigurationLoader
    {
        public static Config Load() => LoadConfig("akka.conf");

        private static Config LoadConfig(string configFile)
        {
            if (File.Exists(configFile))
            {
                var config = File.ReadAllText(configFile);
                return ConfigurationFactory.ParseString(config);
            }
            return Config.Empty;
        }
    }
}
