namespace ChaoticOnyx.Hekate.Parser
{
	public class SyntaxNode
	{
		public SyntaxToken? Token { get; }

		public NodeKind Kind
		{
			get;
			set;
		}

		public SyntaxNode(NodeKind kind)
		{
			Kind = kind;
		}
		
		public SyntaxNode(SyntaxToken token)
		{
			Token = token;
		}

		public SyntaxNode(SyntaxToken token, NodeKind kind) : this(token)
		{
			Kind = kind;
		}
	}
}
