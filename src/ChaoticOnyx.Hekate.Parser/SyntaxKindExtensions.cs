namespace ChaoticOnyx.Hekate.Parser
{
    public static class SyntaxKindExtensions
    {
        public static bool IsKeyword(this SyntaxKind kind)
            => kind switch
            {
                SyntaxKind.ForKeyword    => true,
                SyntaxKind.NewKeyword    => true,
                SyntaxKind.GlobalKeyword => true,
                SyntaxKind.ThrowKeyword  => true,
                SyntaxKind.CatchKeyword  => true,
                SyntaxKind.TryKeyword    => true,
                SyntaxKind.VarKeyword    => true,
                SyntaxKind.VerbKeyword   => true,
                SyntaxKind.ProcKeyword   => true,
                SyntaxKind.InKeyword     => true,
                SyntaxKind.IfKeyword     => true,
                SyntaxKind.ElseKeyword   => true,
                SyntaxKind.SetKeyword    => true,
                SyntaxKind.AsKeyword     => true,
                SyntaxKind.WhileKeyword  => true,
                _                        => false
            };
    }
}
