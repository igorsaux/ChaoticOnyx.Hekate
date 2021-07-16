using System.Collections.Generic;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace ChaoticOnyx.Hekate.Server.Language
{
    public abstract class CodeAnalyzer : SyntaxWalker
    {
        public abstract string          AnalyzerId { get; }
        public          List<CodeIssue> Issues     { get; } = new();
    }
}
