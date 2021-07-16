using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Handlers;
using ChaoticOnyx.Hekate.Server.Language;
using ChaoticOnyx.Hekate.Server.Services.Files;
using ChaoticOnyx.Hekate.Server.Services.Language;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Server;
using Serilog;
using Serilog.Events;

namespace ChaoticOnyx.Hekate.Server
{
    internal sealed class Program
    {
        private static void ConfigureServer(LanguageServerOptions options)
        {
            options.WithInput(Console.OpenStandardInput())
                   .WithOutput(Console.OpenStandardOutput())
                   .ConfigureLogging(builder => builder.AddSerilog(Log.Logger)
                                                       .AddLanguageProtocolLogging()
                                                       .SetMinimumLevel(LogLevel.Debug))
                   .WithServices(services =>
                   {
                       services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Trace));
                       services.AddSingleton<IFileProvider, CachedFileProvider>();
                       services.AddSingleton<IDmLanguageService, DmLanguageService>();
                       services.AddSingleton<IDmEnvironmentService, DmEnvironmentService>();
                   })
                   .WithHandler<ParseEnvironmentHandler>()
                   .WithHandler<ParseFileHandler>();
        }

        private static async Task Main()
        {
            Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
                                                  .WriteTo.File("hekate-server.log", rollingInterval: RollingInterval.Hour)
                                                  .MinimumLevel.Verbose()
                                                  .CreateLogger();

            LanguageServer server = await LanguageServer.From(ConfigureServer);
            await server.WaitForExit;
        }
    }
}
