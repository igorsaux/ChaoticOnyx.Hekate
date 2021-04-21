namespace ChaoticOnyx.Hekate.Parser.SyntaxNodes
{
    public class AssignmentExpression : BinaryExpression
    {
        public AssignmentExpression(ExpressionNode left, ExpressionNode right, SyntaxToken @operator) : base(left, right, @operator) { }
    }
}
