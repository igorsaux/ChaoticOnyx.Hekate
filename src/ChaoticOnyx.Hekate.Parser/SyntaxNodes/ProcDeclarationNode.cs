using System.Collections.Generic;

namespace ChaoticOnyx.Hekate.Parser.SyntaxNodes
{
	public class ProcDeclarationNode : DeclarationNode
	{
		public ProcDeclarationNode(SyntaxToken token, IList<SyntaxToken> fullPath) : base(token, NodeKind.ProcDeclaration, fullPath) { }
	}
}
