#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

#endregion

namespace ChaoticOnyx.Hekate.Parser
{
    /// <summary>
    ///     Лексический анализатор.
    /// </summary>
    public class Lexer
    {
        private readonly List<CodeIssue>   _issues          = new();
        private readonly List<SyntaxToken> _leadTokensCache = new();
        private readonly TextContainer     _source;
        private readonly List<SyntaxToken> _tokens           = new();
        private readonly List<SyntaxToken> _trailTokensCache = new();

        /// <summary>
        ///     Токены в единице компиляции.
        /// </summary>
        public ReadOnlyCollection<SyntaxToken> Tokens => _tokens.AsReadOnly();

        /// <summary>
        ///     Проблемы обнаруженные в единице компиляции.
        /// </summary>
        public ReadOnlyCollection<CodeIssue> Issues => _issues.AsReadOnly();

        /// <summary>
        ///     Создание нового лексера из текста.
        /// </summary>
        /// <param name="source">Исходный код единицы компиляции.</param>
        public Lexer(string source)
        {
            _source = new(source);
        }

        /// <summary>
        ///     Создание нового лексера из набора токенов.
        /// </summary>
        /// <param name="tokens">Набор токенов.</param>
        public Lexer(params SyntaxToken[] tokens)
        {
            _tokens = tokens.ToList();
            _source = new(Emit());
            Parse();
        }

        /// <summary>
        ///     Выполнение лексического парсинга исходного кода. При вызове функции все данные с предыдущего парсинга обнуляются.
        /// </summary>
        public void Parse()
        {
            _tokens.Clear();

            while (true)
            {
                var token = Lex();
                _tokens.Add(token);

                if (token.Kind == SyntaxKind.EndOfFile)
                {
                    return;
                }
            }
        }

        /// <summary>
        ///     Превращение последовательности токенов в текст.
        /// </summary>
        /// <returns></returns>
        public string Emit()
        {
            StringBuilder builder = new();

            foreach (var token in _tokens)
            {
                builder.Append(token.FullText);
            }

            return builder.ToString();
        }

        /// <summary>
        ///     Парсинг одного токена с хвостами и ведущими.
        /// </summary>
        /// <returns></returns>
        private SyntaxToken Lex()
        {
            _leadTokensCache.Clear();
            ParseTokenTrivia(false, _leadTokensCache);
            var token = ScanToken();
            _trailTokensCache.Clear();
            ParseTokenTrivia(true, _trailTokensCache);
            token.AddLeadTokens(_leadTokensCache.ToArray());
            token.AddTrailTokens(_trailTokensCache.ToArray());

            return token;
        }

        /// <summary>
        ///     Создание проблемы в коде.
        /// </summary>
        /// <param name="id">Идентификатор проблемы.</param>
        /// <param name="token">Токен, с которым связана проблема.</param>
        private void MakeIssue(string id, SyntaxToken token)
        {
            MakeIssue(id, token, Array.Empty<object>());
        }

        /// <inheritdoc cref="MakeIssue(ChaoticOnyx.Hekate.Parser.IssueId,ChaoticOnyx.Hekate.Parser.SyntaxToken)" />
        /// <param name="args">Дополнительные аргументы, используются для форматирования сообщения об проблеме.</param>
        private void MakeIssue(string id, SyntaxToken token, params object[] args)
        {
            _issues.Add(new(id, token, _source.FilePosition, args));
        }

        /// <summary>
        ///     Парсинг одного токена.
        /// </summary>
        /// <returns></returns>
        private SyntaxToken ScanToken()
        {
            _source.Start();

            if (_source.IsEnd)
            {
                return CreateTokenAndAdvance(SyntaxKind.EndOfFile, 0);
            }

            var         ch            = _source.Peek();
            var         parsingResult = false;
            SyntaxToken token;
            var         next = _source.Peek(2);

            switch (ch)
            {
                case '/':
                    switch (next)
                    {
                        case '=':
                            return CreateTokenAndAdvance(SyntaxKind.SlashEqual, 2);
                    }

                    return CreateTokenAndAdvance(SyntaxKind.Slash, 1);

                    ;
                case '\\':
                    switch (next)
                    {
                        case '=':
                            return CreateTokenAndAdvance(SyntaxKind.BackwardSlashEqual, 2);
                    }

                    break;
                case '*':
                    return next switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.AsteriskEqual, 2),
                        '*' => CreateTokenAndAdvance(SyntaxKind.DoubleAsterisk, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Asterisk, 1)
                    };
                case '=':
                    return next switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.DoubleEqual, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Equal, 1)
                    };
                case '!':
                    return next switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.ExclamationEqual, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Exclamation, 1)
                    };
                case '>':
                    switch (next)
                    {
                        case '=':
                            return CreateTokenAndAdvance(SyntaxKind.GreaterEqual, 2);
                        case '>':
                            var next2 = _source.Peek(3);

                            return next2 switch
                            {
                                '=' => CreateTokenAndAdvance(SyntaxKind.DoubleGreaterEqual, 3),
                                _   => CreateTokenAndAdvance(SyntaxKind.DoubleGreater, 2)
                            };
                    }

                    return CreateTokenAndAdvance(SyntaxKind.Greater, 1);
                case '<':
                    switch (next)
                    {
                        case '=':
                            return CreateTokenAndAdvance(SyntaxKind.LesserEqual, 2);
                        case '<':
                            var next2 = _source.Peek(3);

                            return next2 switch
                            {
                                '=' => CreateTokenAndAdvance(SyntaxKind.DoubleLesserEqual, 3),
                                _   => CreateTokenAndAdvance(SyntaxKind.DoubleLesser, 2)
                            };
                    }

                    return CreateTokenAndAdvance(SyntaxKind.Lesser, 1);
                case '(':
                    return CreateTokenAndAdvance(SyntaxKind.OpenParenthesis, 1);
                case ')':
                    return CreateTokenAndAdvance(SyntaxKind.CloseParenthesis, 1);
                case '{':
                    return CreateTokenAndAdvance(SyntaxKind.OpenBrace, 1);
                case '}':
                    return CreateTokenAndAdvance(SyntaxKind.CloseBrace, 1);
                case '[':
                    return CreateTokenAndAdvance(SyntaxKind.OpenBracket, 1);
                case ']':
                    return CreateTokenAndAdvance(SyntaxKind.CloseBracket, 1);
                case '+':
                    return next switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.PlusEqual, 2),
                        '+' => CreateTokenAndAdvance(SyntaxKind.DoublePlus, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Plus, 1)
                    };
                case '-':
                    return next switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.MinusEqual, 2),
                        '-' => CreateTokenAndAdvance(SyntaxKind.DoubleMinus, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Minus, 1)
                    };
                case '\'':
                    _source.Advance();
                    parsingResult = ParsePathLiteral();
                    token         = CreateToken(SyntaxKind.PathLiteral);

                    if (!parsingResult)
                    {
                        MakeIssue("DM0001", token, token.Text);
                    }

                    return token;
                case '\"':
                    _source.Advance();
                    parsingResult = ParseTextLiteral();
                    token         = CreateToken(SyntaxKind.TextLiteral);

                    if (!parsingResult)
                    {
                        MakeIssue("DM0001", token, token.Text);
                    }

                    return token;
                case '%':
                    return next switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.PercentEqual, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Percent, 1)
                    };
                case '&':
                    return next switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.AmpersandEqual, 2),
                        '&' => CreateTokenAndAdvance(SyntaxKind.DoubleAmpersand, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Ampersand, 1)
                    };
                case '?':
                    return CreateTokenAndAdvance(SyntaxKind.Question, 1);
                case ':':
                    return CreateTokenAndAdvance(SyntaxKind.Colon, 1);
                case '^':
                    return next switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.CaretEqual, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Caret, 1)
                    };
                case '|':
                    return next switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.BarEqual, 2),
                        '|' => CreateTokenAndAdvance(SyntaxKind.DoubleBar, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Bar, 1)
                    };
                case ',':
                    return CreateTokenAndAdvance(SyntaxKind.Comma, 1);
                case '#':
                    _source.Advance();
                    ParseIdentifier();
                    token = CreateToken(SyntaxKind.Directive);
                    SetDirectiveKind(token);

                    if (token.Kind == SyntaxKind.Directive)
                    {
                        MakeIssue("DM0003", token, token.Text);
                    }

                    return token;
            }

            _source.Advance();

            if (char.IsLetter(ch))
            {
                ParseIdentifier();
                token = CreateToken(SyntaxKind.Identifier);
                SetKeywordOrIdentifierKind(token);

                return token;
            }

            if (char.IsDigit(ch))
            {
                ParseNumericalLiteral();

                return CreateToken(SyntaxKind.NumericalLiteral);
            }

            token = CreateToken(SyntaxKind.Unknown);
            MakeIssue("DM0002", token, token.Text);

            return token;
        }

        /// <summary>
        ///     Определение типа директивы препроцессора.
        /// </summary>
        /// <param name="directive"></param>
        private void SetDirectiveKind(SyntaxToken directive)
        {
            directive.Kind = directive.Text[1..] switch
            {
                "define"  => SyntaxKind.DefineDirective,
                "ifdef"   => SyntaxKind.IfDefDirective,
                "include" => SyntaxKind.IncludeDirective,
                "ifndef"  => SyntaxKind.IfNDefDirective,
                "endif"   => SyntaxKind.EndIfDirective,
                _         => SyntaxKind.Directive
            };
        }

        /// <summary>
        ///     Определение ключевого слова.
        /// </summary>
        /// <param name="identifier"></param>
        private void SetKeywordOrIdentifierKind(SyntaxToken identifier)
        {
            identifier.Kind = identifier.Text switch
            {
                "for"    => SyntaxKind.ForKeyword,
                "new"    => SyntaxKind.NewKeyword,
                "global" => SyntaxKind.GlobalKeyword,
                "throw"  => SyntaxKind.ThrowKeyword,
                "catch"  => SyntaxKind.CatchKeyword,
                "try"    => SyntaxKind.TryKeyword,
                "var"    => SyntaxKind.VarKeyword,
                "verb"   => SyntaxKind.VerbKeyword,
                "proc"   => SyntaxKind.ProcKeyword,
                "in"     => SyntaxKind.InKeyword,
                "if"     => SyntaxKind.IfKeyword,
                "else"   => SyntaxKind.ElseKeyword,
                "set"    => SyntaxKind.SetKeyword,
                "as"     => SyntaxKind.AsKeyword,
                "while"  => SyntaxKind.WhileKeyword,
                _        => SyntaxKind.Identifier
            };
        }

        /// <summary>
        ///     Парсинг идентификатора.
        /// </summary>
        private void ParseIdentifier()
        {
            while (true)
            {
                if (_source.IsEnd)
                {
                    return;
                }

                var ch = _source.Peek();

                if (!char.IsLetter(ch) && ch != '_' && !char.IsDigit(ch))
                {
                    return;
                }

                _source.Advance();
            }
        }

        /// <summary>
        ///     Парсинг числового литерала.
        /// </summary>
        private void ParseNumericalLiteral()
        {
            while (true)
            {
                if (_source.IsEnd)
                {
                    return;
                }

                var ch = _source.Peek();

                if (!char.IsDigit(ch) && ch != '.')
                {
                    return;
                }

                _source.Advance();
            }
        }

        /// <summary>
        ///     Парсинг текстового литерала.
        /// </summary>
        /// <returns></returns>
        private bool ParseTextLiteral()
        {
            while (true)
            {
                if (_source.IsEnd)
                {
                    return false;
                }

                if (_source.Read() == '\"')
                {
                    return true;
                }
            }
        }

        /// <summary>
        ///     Парсинг литерала пути.
        /// </summary>
        /// <returns></returns>
        private bool ParsePathLiteral()
        {
            while (true)
            {
                if (_source.IsEnd)
                {
                    return false;
                }

                if (_source.Read() == '\'')
                {
                    return true;
                }
            }
        }

        private SyntaxToken CreateTokenAndAdvance(SyntaxKind kind, int length)
        {
            _source.Advance(length);

            return new(kind, _source.LexemeText, _source.Position);
        }

        private SyntaxToken CreateToken(SyntaxKind kind)
        {
            return new(kind, _source.LexemeText, _source.Position);
        }

        /// <summary>
        ///     Парсинг ведущих и хвостовых токенов.
        /// </summary>
        /// <param name="isTrail">true - если производится парсинг хвостовых токенов.</param>
        /// <param name="trivia">Лист, куда будут добавлены найденные токены.</param>
        private void ParseTokenTrivia(bool isTrail, List<SyntaxToken> trivia)
        {
            while (true)
            {
                _source.Start();

                if (_source.IsEnd)
                {
                    return;
                }

                var ch   = _source.Peek();
                var next = _source.Peek(2);

                switch (ch)
                {
                    case '/':
                        if (isTrail)
                        {
                            return;
                        }

                        switch (next)
                        {
                            case '/':
                                _source.Advance(2);
                                SkipToEndOfLine();
                                trivia.Add(CreateToken(SyntaxKind.SingleLineComment));

                                break;
                            case '*':
                                _source.Advance(2);
                                var endFounded = SkipToEndOfMultiLineComment();
                                var comment    = CreateToken(SyntaxKind.MultiLineComment);

                                if (!endFounded)
                                {
                                    MakeIssue("DM0001", comment, "/*");
                                }

                                trivia.Add(comment);

                                break;
                            default:
                                return;
                        }

                        break;
                    case ' ':
                    case '\t':
                    case '\v':
                    case '\f':
                    case '\u00A0':
                    case '\uFEFF':
                    case '\u001A':
                        SkipWhiteSpaces();
                        trivia.Add(CreateToken(SyntaxKind.WhiteSpace));

                        break;
                    case '\r':
                        switch (next)
                        {
                            case '\n':
                                trivia.Add(CreateTokenAndAdvance(SyntaxKind.EndOfLine, 2));

                                break;
                        }

                        break;
                    case '\n':
                        trivia.Add(CreateTokenAndAdvance(SyntaxKind.EndOfLine, 1));

                        break;
                    case '.':
                        trivia.Add(CreateTokenAndAdvance(SyntaxKind.Dot, 1));

                        break;
                    default:
                        return;
                }
            }
        }

        /// <summary>
        ///     Пропуск пустот, пробелов, табуляции и т.д.
        /// </summary>
        private void SkipWhiteSpaces()
        {
            while (!_source.IsEnd)
            {
                var ch = _source.Peek();

                if (ch != ' ' && ch != '\t')
                {
                    return;
                }

                _source.Advance();
            }
        }

        /// <summary>
        ///     Пропуск до конца многострочного комментария.
        /// </summary>
        /// <returns></returns>
        private bool SkipToEndOfMultiLineComment()
        {
            while (true)
            {
                if (_source.IsEnd)
                {
                    return false;
                }

                var ch = _source.Read();

                switch (ch)
                {
                    case '*':
                        if (_source.Peek() == '/')
                        {
                            _source.Advance();

                            return true;
                        }

                        break;
                }
            }
        }

        /// <summary>
        ///     Пропуск до конца однострочного комментария.
        /// </summary>
        private void SkipToEndOfLine()
        {
            while (!_source.IsEnd)
            {
                var ch = _source.Peek();

                if (ch == '\n')
                {
                    return;
                }

                _source.Advance();
            }
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            foreach (var token in _tokens)
            {
                result.Append($"{token.Text}");
            }

            return result.ToString();
        }
    }
}
