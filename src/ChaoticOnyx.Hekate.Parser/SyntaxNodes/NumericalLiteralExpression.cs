using System.Diagnostics;

namespace ChaoticOnyx.Hekate.Parser.SyntaxNodes
{
    public class NumericalLiteralExpression : ExpressionNode
    {
        public NumericalLiteralExpression(SyntaxToken literal) : base(literal) => Debug.Assert(literal.Kind == SyntaxKind.NumericalLiteral, $"{nameof(literal)} is not numerical literal.");
    }
}
