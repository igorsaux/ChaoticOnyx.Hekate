namespace ChaoticOnyx.Hekate.Parser.SyntaxNodes
{
    public class BinaryExpression : ExpressionNode
    {
        public ExpressionNode Left     { get; }
        public ExpressionNode Right    { get; }
        public SyntaxToken    Operator { get; }

        public BinaryExpression(ExpressionNode left, ExpressionNode right, SyntaxToken @operator) : base(null)
        {
            Left     = left;
            Right    = right;
            Operator = @operator;
        }
    }
}
