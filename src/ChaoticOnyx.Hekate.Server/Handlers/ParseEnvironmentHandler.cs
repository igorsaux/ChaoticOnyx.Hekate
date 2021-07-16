using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Services.Language;
using MediatR;
using OmniSharp.Extensions.JsonRpc;
using Serilog;

namespace ChaoticOnyx.Hekate.Server.Handlers
{
    public class ParseEnvironmentHandler : IParseEnvironmentHandlerBase
    {
        private readonly IDmEnvironmentService _dmEnvironment;

        public ParseEnvironmentHandler(IDmEnvironmentService dmEnvironment)
        {
            _dmEnvironment = dmEnvironment;
        }

        public async Task<Unit> Handle(ParseEnvironmentRequest request, CancellationToken cancellationToken)
        {
            Log.Logger.Debug($"Парсинг {request.DmeUri}");
            await _dmEnvironment.ParseEnvironmentAsync(new FileInfo(request.DmeUri), cancellationToken);

            return Unit.Value;
        }
    }
}
