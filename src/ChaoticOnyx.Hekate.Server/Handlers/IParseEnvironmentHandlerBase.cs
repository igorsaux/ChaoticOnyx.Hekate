using MediatR;
using OmniSharp.Extensions.JsonRpc;

namespace ChaoticOnyx.Hekate.Server.Handlers
{
    public sealed record ParseEnvironmentRequest : IRequest
    {
        public string DmeUri { get; set; } = string.Empty;
    }

    [Parallel]
    [Method("hekate/parseEnvironment")]
    public interface IParseEnvironmentHandlerBase : IJsonRpcRequestHandler<ParseEnvironmentRequest> { }
}
