using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ChaoticOnyx.Hekate
{
    public sealed class Preprocessor
    {
        private readonly List<SyntaxToken>          _defines  = new();
        private readonly Stack<SyntaxToken>         _ifs      = new();
        private readonly List<SyntaxToken>          _includes = new();
        private readonly List<CodeIssue>            _issues   = new();
        private readonly TypeContainer<SyntaxToken> _tokens;

        public IReadOnlyCollection<CodeIssue> Issues   => _issues.AsReadOnly();
        public IImmutableList<SyntaxToken>    Includes => _includes.ToImmutableList();
        public IImmutableList<SyntaxToken>    Defines  => _defines.ToImmutableList();

        private Preprocessor(IImmutableList<SyntaxToken>? tokens = null) => _tokens = new TypeContainer<SyntaxToken>(tokens?.ToList() ?? new List<SyntaxToken>());

        public static Preprocessor WithTokens(IImmutableList<SyntaxToken> tokens) => new(tokens);

        public static Preprocessor WithoutTokens() => new();

        /// <summary>
        ///     Производит препроцессинг токенов.
        /// </summary>
        public void Preprocess()
        {
            _ifs.Clear();
            _defines.Clear();
            _includes.Clear();
            _issues.Clear();

            while (!_tokens.IsEnd)
            {
                SyntaxToken  token = _tokens.Read();
                SyntaxToken? next  = _tokens.Peek();

                if (next is null)
                {
                    break;
                }

                switch (token.Kind)
                {
                    case SyntaxKind.DefineDirective:
                        _defines.Add(next);

                        break;
                    case SyntaxKind.IncludeDirective:
                        _includes.Add(next);

                        break;
                    case SyntaxKind.IfDefDirective:
                        _ifs.Push(token);

                        if (_defines.Any(t => t.Text == next.Text))
                        {
                            break;
                        }

                        SkipIf();

                        break;
                    case SyntaxKind.IfNDefDirective:
                        _ifs.Push(token);

                        if (_defines.Count == 0 || _defines.Any(t => t.Text != next.Text))
                        {
                            break;
                        }

                        SkipIf();

                        break;
                    case SyntaxKind.UndefDirective:
                        SyntaxToken? define = _defines.Find(t => t.Text == next.Text);

                        if (define != null)
                        {
                            _defines.Remove(define);
                        }

                        _issues.Add(new CodeIssue(IssuesId.UnknownMacrosDefinition, next, next.Text));

                        break;
                    case SyntaxKind.EndIfDirective:
                        if (_ifs.Count == 0)
                        {
                            _issues.Add(new CodeIssue(IssuesId.ExtraEndIf, token));

                            break;
                        }

                        _ifs.Pop();

                        break;
                    case SyntaxKind.ElseDirective:
                        if (_ifs.Count == 0)
                        {
                            _issues.Add(new CodeIssue(IssuesId.UnexpectedElse, token));
                        }

                        continue;
                    default:
                        continue;
                }
            }

            if (_ifs.Count <= 0)
            {
                return;
            }

            SyntaxToken last = _ifs.Last();
            _issues.Add(new CodeIssue(IssuesId.EndIfNotFound, last, last.Text));
        }

        /// <summary>
        ///     Пропускает все токены до первого нахождение #endif или #else.
        /// </summary>
        /// <returns>Возвращает true - если #endif был найден.</returns>
        private void SkipIf()
        {
            while (!_tokens.IsEnd)
            {
                SyntaxToken? token = _tokens.Peek();

                if (token == null)
                {
                    return;
                }

                if (token.Kind is SyntaxKind.EndIfDirective or SyntaxKind.ElseDirective)
                {
                    return;
                }

                _tokens.Advance();
            }
        }
    }
}
