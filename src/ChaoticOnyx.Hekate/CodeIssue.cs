namespace ChaoticOnyx.Hekate
{
    /// <summary>
    ///     Проблемы встречаемые в коде.
    /// </summary>
    public sealed record CodeIssue(string Id, string Message, IssueSeverity Severity, SyntaxToken Token, params object[] MessageArguments);
}
