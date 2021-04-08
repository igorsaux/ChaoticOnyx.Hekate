using System;
using System.Collections.Generic;

namespace ChaoticOnyx.Hekate.Parser.SyntaxNodes
{
	public sealed class TypeDeclarationNode : DeclarationNode
	{
		public TypeDeclarationNode(SyntaxToken token, IList<SyntaxToken> fullPath) :
			base(token, NodeKind.TypeDeclaration, fullPath) { }
	}
}
