using MediatR;
using OmniSharp.Extensions.JsonRpc;

namespace ChaoticOnyx.Hekate.Server.Handlers
{
    public sealed record ParseFileRequest : IRequest
    {
        public string Uri { get; set; } = string.Empty;
    }

    [Parallel]
    [Method("hekate/parseFile")]
    public interface IParseFileHandlerBase : IJsonRpcRequestHandler<ParseFileRequest> { }
}
