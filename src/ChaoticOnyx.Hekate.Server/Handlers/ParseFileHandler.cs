using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ChaoticOnyx.Hekate.Server.Services.Language;
using MediatR;
using OmniSharp.Extensions.JsonRpc;

namespace ChaoticOnyx.Hekate.Server.Handlers
{
    public class ParseFileHandler : IParseFileHandlerBase
    {
        private readonly IDmEnvironmentService _dmEnvironment;

        public ParseFileHandler(IDmEnvironmentService dmEnvironment)
        {
            _dmEnvironment = dmEnvironment;
        }

        public async Task<Unit> Handle(ParseFileRequest request, CancellationToken cancellationToken)
        {
            await _dmEnvironment.ParseFileAsync(new FileInfo(request.Uri), cancellationToken);

            return Unit.Value;
        }
    }
}
