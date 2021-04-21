using System.Collections.Generic;

namespace ChaoticOnyx.Hekate.Parser.SyntaxNodes
{
    public abstract class DeclarationNode : SyntaxNode
    {
        public string             Name     => Token?.Text ?? string.Empty;
        public IList<SyntaxToken> FullPath { get; }

        protected DeclarationNode(SyntaxToken token) : base(token) => FullPath = new List<SyntaxToken>();

        protected DeclarationNode(SyntaxToken token, IList<SyntaxToken> fullPath) : base(token) => FullPath = fullPath;
    }
}
