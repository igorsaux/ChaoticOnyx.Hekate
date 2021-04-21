using System.Collections.Generic;

namespace ChaoticOnyx.Hekate.Parser.SyntaxNodes
{
    public sealed class CompilationUnitNode : SyntaxNode
    {
        private readonly List<DeclarationNode>  _declarations = new();
        public           IList<DeclarationNode> Declarations => _declarations;

        public CompilationUnitNode() : base(null) { }

        public void AddDeclaration(DeclarationNode declaration) => _declarations.Add(declaration);
    }
}
