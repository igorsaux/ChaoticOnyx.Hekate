using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Dm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.WorkDone;
using OmniSharp.Extensions.LanguageServer.Server;
using Serilog;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;

namespace ChaoticOnyx.Hekate.Server
{
    internal class Program
    {
        private static ProjectEnvironment? s_environment;

        private static FileInfo FindDme(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();
            FileInfo?   dme   = files.FirstOrDefault(file => file.Extension == ".dme");

            if (dme is null)
            {
                throw new FileNotFoundException(".dme файл не найден.");
            }

            return dme;
        }

        private static void ConfigureServer(LanguageServerOptions options)
            => options.WithInput(Console.OpenStandardInput())
                      .WithOutput(Console.OpenStandardOutput())
                      .ConfigureLogging(logging => logging.AddSerilog(Log.Logger)
                                                          .AddLanguageProtocolLogging()
                                                          .SetMinimumLevel(LogLevel.Trace))
                      .WithHandler<TextDocumentHandler>()
                      .WithServices(services => services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Trace)))
                      .OnStarted(async (server, token) =>
                      {
                          using IWorkDoneObserver manager = await server.WorkDoneManager.Create(new WorkDoneProgressBegin
                          {
                              Title = "Hekate", Percentage = 0, Message = "Поиск файла среды..."
                          }, cancellationToken: token);

                          s_environment = new ProjectEnvironment(FindDme(new DirectoryInfo(server.Workspace.ClientSettings.RootPath)));

                          manager.OnNext(new WorkDoneProgressReport
                          {
                              Message = "Парсинг проекта...", Percentage = 50
                          });

                          s_environment.Parse();
                          manager.OnNext(new WorkDoneProgressEnd());

                          foreach (var file in s_environment.Files)
                          {
                              List<CodeIssue>  issues      = file.Issues ?? new List<CodeIssue>();
                              List<Diagnostic> diagnostics = new();

                              foreach (var issue in issues)
                              {
                                  FileLine position = issue.Token.FilePosition;

                                  diagnostics.Add(new Diagnostic
                                  {
                                      Code     = issue.Id,
                                      Severity = DiagnosticSeverity.Warning,
                                      Message  = string.Format(Issues.IdToMessage(issue.Id), issue.Arguments),
                                      Range    = new Range(position.Line - 1, position.Column, position.Line - 1, position.Column),
                                      Source   = "Hekate"
                                  });
                              }

                              server.PublishDiagnostics(new PublishDiagnosticsParams
                              {
                                  Uri = file.File.FullName, Diagnostics = Container.From(diagnostics)
                              });
                          }
                      });

        private static async Task Main()
        {
            Log.Logger = new LoggerConfiguration().Enrich.FromLogContext()
                                                  .WriteTo.File("server-log.log", rollingInterval: RollingInterval.Day)
                                                  .MinimumLevel.Verbose()
                                                  .CreateLogger();

            LanguageServer server = await LanguageServer.From(ConfigureServer);
            await server.WaitForExit;
        }
    }
}
