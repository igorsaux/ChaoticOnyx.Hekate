#region

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace ChaoticOnyx.Hekate
{
    /// <summary>
    ///     Лексический анализатор.
    /// </summary>
    public class Lexer
    {
        private readonly List<SyntaxToken> _leadTokensCache  = new();
        private readonly List<SyntaxToken> _trailTokensCache = new();
        private          TextContainer     _source           = new(ReadOnlyMemory<char>.Empty);

        /// <summary>
        ///     Токены в единице компиляции.
        /// </summary>
        public LinkedList<SyntaxToken> Tokens { get; private set; } = new();

        /// <summary>
        ///     Проблемы обнаруженные в единице компиляции.
        /// </summary>
        public List<CodeIssue> Issues { get; private set; } = new();

        /// <summary>
        ///     Определение типа директивы препроцессора.
        /// </summary>
        /// <param name="directive"></param>
        private static void SetDirectiveKind(SyntaxToken directive)
            => directive.Kind = directive.Text[1..] switch
            {
                "define"  => SyntaxKind.DefineDirective,
                "ifdef"   => SyntaxKind.IfDefDirective,
                "include" => SyntaxKind.IncludeDirective,
                "ifndef"  => SyntaxKind.IfNDefDirective,
                "endif"   => SyntaxKind.EndIfDirective,
                "undef"   => SyntaxKind.UndefDirective,
                "else"    => SyntaxKind.ElseDirective,
                "warning" => SyntaxKind.WarningDirective,
                "error"   => SyntaxKind.ErrorDirective,
                "if"      => SyntaxKind.IfDirective,
                "elif"    => SyntaxKind.ElifDirective,
                _         => SyntaxKind.Directive
            };

        /// <summary>
        ///     Определение ключевого слова.
        /// </summary>
        /// <param name="identifier"></param>
        private static void SetKeywordOrIdentifierKind(SyntaxToken identifier)
            => identifier.Kind = identifier.Text switch
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
                "return" => SyntaxKind.ReturnKeyword,
                _        => SyntaxKind.Identifier
            };

        /// <summary>
        ///     Выполнение лексического парсинга исходного кода. При вызове функции старый лист очищается.
        /// </summary>
        public void Parse(ReadOnlyMemory<char> source)
        {
            Issues  = new List<CodeIssue>();
            _source = new TextContainer(source);
            Tokens  = new LinkedList<SyntaxToken>();

            while (true)
            {
                SyntaxToken token = Lex();
                Tokens.AddLast(token);

                if (token.Kind == SyntaxKind.EndOfFile)
                {
                    return;
                }
            }
        }

        /// <summary>
        ///     Парсинг одного токена с хвостами и ведущими.
        /// </summary>
        /// <returns></returns>
        private SyntaxToken Lex()
        {
            _leadTokensCache.Clear();
            ParseTokenTrivia(false, _leadTokensCache);
            SyntaxToken token = ScanToken();
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
        private void MakeIssue(string id, SyntaxToken token) => MakeIssue(id, token, Array.Empty<object>());

        /// <summary>
        ///     Создание проблемы в коде.
        /// </summary>
        /// <param name="id">Идентификатор проблемы.</param>
        /// <param name="token">Токен, с которым связана проблема.</param>
        /// <param name="args">Дополнительные аргументы, используются для форматирования сообщения об проблеме.</param>
        private void MakeIssue(string id, SyntaxToken token, params object[] args) => Issues.Add(new CodeIssue(id, token, args));

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

            if (Tokens.Last?.Value.Kind is SyntaxKind.WarningDirective or SyntaxKind.ErrorDirective)
            {
                SkipToEndOfLine();

                return CreateToken(SyntaxKind.TextLiteral);
            }

            ReadOnlySpan<char> span = _source.Peek();
            bool               parsingResult;
            SyntaxToken        token;
            ReadOnlySpan<char> spanNext = _source.Peek(2);

            switch (span[0])
            {
                case '/':
                    return spanNext[0] switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.SlashEqual, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Slash, 1)
                    };
                case '\\':
                    return spanNext[0] switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.BackSlashEqual, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Backslash, 1)
                    };
                case '*':
                    return spanNext[0] switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.AsteriskEqual, 2),
                        '*' => CreateTokenAndAdvance(SyntaxKind.DoubleAsterisk, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Asterisk, 1)
                    };
                case '=':
                    return spanNext[0] switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.DoubleEqual, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Equal, 1)
                    };
                case '!':
                    return spanNext[0] switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.ExclamationEqual, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Exclamation, 1)
                    };
                case '>':
                    switch (spanNext[0])
                    {
                        case '=':
                            return CreateTokenAndAdvance(SyntaxKind.GreaterEqual, 2);
                        case '>':
                            ReadOnlySpan<char> spanNextNext = _source.Peek(3);

                            return spanNextNext[0] switch
                            {
                                '=' => CreateTokenAndAdvance(SyntaxKind.DoubleGreaterEqual, 3),
                                _   => CreateTokenAndAdvance(SyntaxKind.DoubleGreater, 2)
                            };
                    }

                    return CreateTokenAndAdvance(SyntaxKind.Greater, 1);
                case '<':
                    switch (spanNext[0])
                    {
                        case '=':
                            return CreateTokenAndAdvance(SyntaxKind.LesserEqual, 2);
                        case '<':
                            ReadOnlySpan<char> spanNextNext = _source.Peek(3);

                            return spanNextNext[0] switch
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
                    if (spanNext[0] != '"')
                    {
                        return CreateTokenAndAdvance(SyntaxKind.OpenBrace, 1);
                    }

                    // WARNING: CRINGE AHEAD
                    // Люмокс, ебать спасибо тебе нахуй за три способа сделать строку 😘.
                    _source.Advance();
                    parsingResult = ParseDocumentTextLiteral();
                    token         = CreateToken(SyntaxKind.TextLiteral);

                    if (!parsingResult)
                    {
                        MakeIssue(IssuesId.MissingClosingSign, token, token.Text[0]);
                    }

                    return token;
                case '}':
                    return CreateTokenAndAdvance(SyntaxKind.CloseBrace, 1);
                case '[':
                    return CreateTokenAndAdvance(SyntaxKind.OpenBracket, 1);
                case ']':
                    return CreateTokenAndAdvance(SyntaxKind.CloseBracket, 1);
                case '+':
                    return spanNext[0] switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.PlusEqual, 2),
                        '+' => CreateTokenAndAdvance(SyntaxKind.DoublePlus, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Plus, 1)
                    };
                case '-':
                    return spanNext[0] switch
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
                        MakeIssue(IssuesId.MissingClosingSign, token, token.Text[0]);
                    }

                    return token;
                case '\"':
                    _source.Advance();
                    parsingResult = ParseTextLiteral(span);
                    token         = CreateToken(SyntaxKind.TextLiteral);

                    if (!parsingResult)
                    {
                        MakeIssue(IssuesId.MissingClosingSign, token, token.Text[0]);
                    }

                    return token;
                case '%':
                    return spanNext[0] switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.PercentEqual, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Percent, 1)
                    };
                case '&':
                    return spanNext[0] switch
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
                    return spanNext[0] switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.CaretEqual, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Caret, 1)
                    };
                case '|':
                    return spanNext[0] switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.BarEqual, 2),
                        '|' => CreateTokenAndAdvance(SyntaxKind.DoubleBar, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Bar, 1)
                    };
                case ',':
                    return CreateTokenAndAdvance(SyntaxKind.Comma, 1);
                case '#':
                    _source.Advance();

                    if (spanNext[0] == '#')
                    {
                        _source.Advance();

                        return CreateToken(SyntaxKind.ConcatDirective);
                    }

                    ParseIdentifier();
                    token = CreateToken(SyntaxKind.Directive);
                    SetDirectiveKind(token);

                    if (token.Kind == SyntaxKind.Directive)
                    {
                        token.Kind = SyntaxKind.Identifier;
                    }

                    return token;
                case ';':
                    return CreateTokenAndAdvance(SyntaxKind.Semicolon, 1);
                case '~':
                    return spanNext[0] switch
                    {
                        '=' => CreateTokenAndAdvance(SyntaxKind.TildaEqual, 2),
                        '~' => CreateTokenAndAdvance(SyntaxKind.TildaExclamation, 2),
                        _   => CreateTokenAndAdvance(SyntaxKind.Tilda, 1)
                    };
                case '@':
                    _source.Advance();

                    if (_source.IsEnd)
                    {
                        return CreateToken(SyntaxKind.At);
                    }

                    _source.Advance();
                    parsingResult = ParseRawTextLiteral(GetClosingPairSymbol(spanNext));
                    token         = CreateToken(SyntaxKind.TextLiteral);

                    if (!parsingResult)
                    {
                        MakeIssue(IssuesId.MissingClosingSign, token, token.Text[0]);
                    }

                    return token;
            }

            _source.Advance();

            if (char.IsLetter(span[0]) || span[0] == '_')
            {
                ParseIdentifier();
                token = CreateToken(SyntaxKind.Identifier);
                SetKeywordOrIdentifierKind(token);

                return token;
            }

            if (char.IsDigit(span[0]))
            {
                ParseNumericalLiteral();

                return CreateToken(SyntaxKind.NumericalLiteral);
            }

            token = CreateToken(SyntaxKind.Unknown);
            MakeIssue(IssuesId.UnexpectedToken, token, token.Text);

            return token;
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

                ReadOnlySpan<char> span = _source.Peek();

                if (!char.IsLetter(span[0]) && span[0] != '_' && !char.IsDigit(span[0]))
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

                ReadOnlySpan<char> span = _source.Peek();

                if (!char.IsDigit(span[0]) && span[0] != '.')
                {
                    return;
                }

                _source.Advance();
            }
        }

        /// <summary>
        ///     Парсинг текстового литерала. Учитывает интерполяцию.
        /// </summary>
        /// <param name="closingSign">Закрывающий символ до которого необходимо парсить строку.</param>
        /// <returns>true - если строка была полностью распарсена.</returns>
        private bool ParseTextLiteral(ReadOnlySpan<char> closingSign)
        {
            bool isInterpolating    = false;
            int  interpolatingLevel = 0;
            int  escapeSymbols      = 0;

            while (true)
            {
                if (_source.IsEnd)
                {
                    return false;
                }

                bool               escaped = escapeSymbols != 0 && (escapeSymbols == 1 || escapeSymbols % 2 != 0);
                ReadOnlySpan<char> span    = _source.Read();

                if (span[0] == '\\')
                {
                    escapeSymbols++;
                }
                else
                {
                    escapeSymbols = 0;
                }

                if (escaped)
                {
                    continue;
                }

                switch (span[0])
                {
                    case '[':
                        interpolatingLevel++;
                        isInterpolating = true;

                        break;
                    case ']' when interpolatingLevel > 0:
                        {
                            interpolatingLevel--;

                            if (interpolatingLevel == 0)
                            {
                                isInterpolating = false;
                            }

                            break;
                        }
                }

                if (!isInterpolating && span[0] == closingSign[0])
                {
                    return true;
                }
            }
        }

        /// <summary>
        ///     Оптимизированный вариант функции ParseTextLiteral. Не учитывает возможную интерполяцию.
        /// </summary>
        /// <param name="closingSign">Закрывающий символ до которого необходимо парсить строку.</param>
        /// <returns></returns>
        private bool ParseRawTextLiteral(ReadOnlySpan<char> closingSign)
        {
            int escapeSymbols = 0;

            while (true)
            {
                if (_source.IsEnd)
                {
                    return false;
                }

                bool               escaped = escapeSymbols != 0 && (escapeSymbols == 1 || escapeSymbols % 2 != 0);
                ReadOnlySpan<char> span    = _source.Read();

                if (span[0] == '\\')
                {
                    escapeSymbols++;
                }
                else
                {
                    escapeSymbols = 0;
                }

                if (!escaped && span[0] == closingSign[0])
                {
                    return true;
                }
            }
        }

        private bool ParseDocumentTextLiteral()
        {
            bool isInterpolating    = false;
            int  interpolatingLevel = 0;
            int  indentLevel        = 1;
            int  escapeSymbols      = 0;

            while (true)
            {
                if (_source.IsEnd)
                {
                    return false;
                }

                bool               escaped = escapeSymbols != 0 && (escapeSymbols == 1 || escapeSymbols % 2 != 0);
                ReadOnlySpan<char> span    = _source.Read();

                if (span[0] == '\\')
                {
                    escapeSymbols++;
                }
                else
                {
                    escapeSymbols = 0;
                }

                if (escaped)
                {
                    continue;
                }

                switch (span[0])
                {
                    case '[':
                        interpolatingLevel++;
                        isInterpolating = true;

                        break;
                    case ']' when interpolatingLevel > 0:
                        {
                            interpolatingLevel--;

                            if (interpolatingLevel == 0)
                            {
                                isInterpolating = false;
                            }

                            break;
                        }
                    case '{':
                        indentLevel++;

                        break;
                    case '}':
                        indentLevel--;

                        break;
                }

                if (!isInterpolating && indentLevel == 0 && span[0] == '}')
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

                if (_source.Read()[0] == '\'')
                {
                    return true;
                }
            }
        }

        private SyntaxToken CreateTokenAndAdvance(SyntaxKind kind, int length)
        {
            _source.Advance(length);

            return new SyntaxToken(kind, _source.LexemeText, _source.Position, _source.LexemeFilePosition);
        }

        private SyntaxToken CreateToken(SyntaxKind kind) => new(kind, _source.LexemeText, _source.Position, _source.LexemeFilePosition);

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

                ReadOnlySpan<char> span     = _source.Peek();
                ReadOnlySpan<char> spanNext = _source.Peek(2);

                switch (span[0])
                {
                    case '/':
                        if (isTrail)
                        {
                            return;
                        }

                        switch (spanNext[0])
                        {
                            case '/':
                                _source.Advance(2);
                                SkipToEndOfLine();
                                trivia.Add(CreateToken(SyntaxKind.SingleLineComment));

                                break;
                            case '*':
                                _source.Advance(2);
                                bool        endFounded = SkipToEndOfMultiLineComment();
                                SyntaxToken comment    = CreateToken(SyntaxKind.MultiLineComment);

                                if (!endFounded)
                                {
                                    MakeIssue(IssuesId.MissingClosingSign, comment, "/*");
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
                        switch (spanNext[0])
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
                ReadOnlySpan<char> span = _source.Peek();

                if (span[0] != ' ' && span[0] != '\t')
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

                ReadOnlySpan<char> span = _source.Read();

                switch (span[0])
                {
                    case '*':
                        ReadOnlySpan<char> spanNext = _source.Peek();

                        if (spanNext.IsEmpty)
                        {
                            return false;
                        }

                        if (spanNext[0] == '/')
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
                ReadOnlySpan<char> span = _source.Peek();

                if (span[0] == '\n')
                {
                    return;
                }

                _source.Advance();
            }
        }

        public override string ToString()
        {
            StringBuilder result = new();

            foreach (var token in Tokens)
            {
                result.Append($"{token.Text}");
            }

            return result.ToString();
        }

        /// <summary>
        ///     Возвращает закрывающий символ для пары.
        /// </summary>
        /// <returns></returns>
        private ReadOnlySpan<char> GetClosingPairSymbol(ReadOnlySpan<char> openPair)
        {
            switch (openPair[0])
            {
                case '[':
                    return "]";
                case '{':
                    return "}";
                case '(':
                    return ")";
                case '<':
                    return ">";
                default:
                    return openPair;
            }
        }
    }
}
