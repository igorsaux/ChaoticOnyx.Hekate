using System;
using System.Collections.Generic;
using System.Text;

namespace ChaoticOnyx.Hekate.Scaffolds
{
    public sealed class TokensToTextScaffold : CodeScaffold<Memory<char>>
    {
        private readonly LinkedList<SyntaxToken> _tokens;

        public TokensToTextScaffold(LinkedList<SyntaxToken> tokens, Lexer? lexer = null) : base(lexer ?? new Lexer()) => _tokens = tokens;

        public override Memory<char> GetResult()
        {
            StringBuilder sb = new(_tokens.Count);

            foreach (var token in _tokens)
            {
                sb.Append(token.FullText);
            }

            return new Memory<char>(sb.ToString()
                                      .ToCharArray());
        }
    }
}
