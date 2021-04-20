namespace ChaoticOnyx.Hekate.Parser.SyntaxNodes
{
	public abstract class ExpressionNode : SyntaxNode
	{
		protected ExpressionNode(SyntaxToken? token) : base(token) { }
	}
}
