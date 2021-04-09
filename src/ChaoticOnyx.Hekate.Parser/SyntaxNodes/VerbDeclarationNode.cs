using System.Collections.Generic;

namespace ChaoticOnyx.Hekate.Parser.SyntaxNodes
{
	public class VerbDeclarationNode : DeclarationNode
	{
		public VerbDeclarationNode(SyntaxToken token, IList<SyntaxToken> fullPath) : base(token, NodeKind.VerbDeclaration, fullPath) { }
	}
}
