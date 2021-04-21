using System.Collections.Generic;

namespace ChaoticOnyx.Hekate.Parser.SyntaxNodes
{
    public sealed class VariableDeclarationNode : DeclarationNode
    {
        public VariableDeclarationNode(SyntaxToken token, IList<SyntaxToken> fullPath) : base(token, fullPath) { }
    }
}
