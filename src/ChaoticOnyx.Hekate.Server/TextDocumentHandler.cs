using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Document;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace ChaoticOnyx.Hekate.Server
{
    public class TextDocumentHandler : ITextDocumentSyncHandler
    {
        private readonly DocumentSelector _selector = new(new DocumentFilter
        {
            Pattern = "**/*.dm"
        });

        public Task<Unit> Handle(DidChangeTextDocumentParams request, CancellationToken cancellationToken) => Unit.Task;

        TextDocumentChangeRegistrationOptions IRegistration<TextDocumentChangeRegistrationOptions, SynchronizationCapability>.GetRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
            => new()
            {
                DocumentSelector = _selector
            };

        public Task<Unit> Handle(DidOpenTextDocumentParams request, CancellationToken cancellationToken) => Unit.Task;

        TextDocumentOpenRegistrationOptions IRegistration<TextDocumentOpenRegistrationOptions, SynchronizationCapability>.GetRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
            => new()
            {
                DocumentSelector = _selector
            };

        public Task<Unit> Handle(DidCloseTextDocumentParams request, CancellationToken cancellationToken) => Unit.Task;

        TextDocumentCloseRegistrationOptions IRegistration<TextDocumentCloseRegistrationOptions, SynchronizationCapability>.GetRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
            => new()
            {
                DocumentSelector = _selector
            };

        public Task<Unit> Handle(DidSaveTextDocumentParams request, CancellationToken cancellationToken) => Unit.Task;

        public TextDocumentAttributes GetTextDocumentAttributes(DocumentUri uri) => new(uri, "dm");

        public TextDocumentSaveRegistrationOptions GetRegistrationOptions(SynchronizationCapability capability, ClientCapabilities clientCapabilities)
            => new()
            {
                DocumentSelector = _selector, IncludeText = false
            };
    }
}
