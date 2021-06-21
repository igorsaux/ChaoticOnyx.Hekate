using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ChaoticOnyx.Hekate
{
    /// <summary>
    ///     Препроцессирует набор токенов и возвращает итоговый контекст.
    /// </summary>
    public sealed class Preprocessor
    {
        private readonly Stack<SyntaxToken>         _ifs      = new();
        private readonly List<CodeIssue>            _issues   = new();
        private          List<SyntaxToken>          _defines  = new();
        private          List<SyntaxToken>          _includes = new();
        private          TypeContainer<SyntaxToken> _tokens   = new();

        public IImmutableList<CodeIssue> Issues => _issues.ToImmutableList();

        /// <summary>
        ///     Текущий контекст препроцессора.
        /// </summary>
        public PreprocessorContext Context => new(_includes.ToImmutableList(), _defines.ToImmutableList());

        /// <summary>
        ///     Производит препроцессинг токенов.
        /// </summary>
        public PreprocessorContext Preprocess(IImmutableList<SyntaxToken> tokens, PreprocessorContext? context = null)
        {
            _tokens       = new TypeContainer<SyntaxToken>(tokens.ToList());
            (_, _defines) = context ?? PreprocessorContext.Empty;
            _includes.Clear();
            _issues.Clear();
            _ifs.Clear();

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

                            break;
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
                return new PreprocessorContext(_includes.ToImmutableList(), _defines.ToImmutableList());
            }

            SyntaxToken last = _ifs.Last();
            _issues.Add(new CodeIssue(IssuesId.EndIfNotFound, last, last.Text));

            return new PreprocessorContext(_includes.ToImmutableList(), _defines.ToImmutableList());
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
