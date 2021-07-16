using System;
using System.Collections.Generic;

namespace ChaoticOnyx.Hekate.Scaffolds
{
    /// <summary>
    ///     Конвертирует текстовое представление в набор синтаксических токенов.
    /// </summary>
    public sealed class TextToTokensScaffold : CodeScaffold<(List<CodeIssue>, LinkedList<SyntaxToken>)>
    {
        private readonly ReadOnlyMemory<char> _text;

        public TextToTokensScaffold(ReadOnlyMemory<char> text, Lexer? lexer = null) : base(lexer ?? new Lexer()) => _text = text;

        public override (List<CodeIssue>, LinkedList<SyntaxToken>) GetResult()
        {
            var result = Lexer.Parse(_text);

            return result;
        }
    }
}
