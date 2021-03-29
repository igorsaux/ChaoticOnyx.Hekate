namespace ChaoticOnyx.Hekate.Parser
{
    /// <summary>
    ///     Проблемы встречаемые в коде.
    /// </summary>
    public record CodeIssue
    {
        public readonly object[]    Arguments;
        public readonly string      Id;
        public readonly SyntaxToken Token;

        /// <summary>
        ///     Создаёт новую запись о проблеме в коде.
        /// </summary>
        /// <param name="id">Идентификатор проблемы.</param>
        /// <param name="token">Токен, с которым связана проблема.</param>
        /// <param name="args">Дополнительные аргументы, используются при выводе сообщения о проблеме.</param>
        public CodeIssue(string id, SyntaxToken token, params object[] args)
        {
            Id        = id;
            Token     = token;
            Arguments = args;
        }
    }
}
