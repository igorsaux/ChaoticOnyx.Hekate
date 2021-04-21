namespace ChaoticOnyx.Hekate.Parser
{
    public class SyntaxNode
    {
        public SyntaxToken? Token { get; }

        public SyntaxNode(SyntaxToken? token) => Token = token;
    }
}
