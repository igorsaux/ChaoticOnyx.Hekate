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
        private List<CodeIssue>              _issues = new();
        private          LinkedListNode<SyntaxToken>? _it;
        private          LinkedList<SyntaxToken>      _tokens  = new();
        private          PreprocessorContext          _context = null!;

        public List<CodeIssue> Issues => _issues;

        /// <summary>
        ///     Текущий контекст препроцессора.
        /// </summary>
        public PreprocessorContext Context => _context;

        /// <summary>
        ///     Производит препроцессинг токенов.
        /// </summary>
        public PreprocessorContext Preprocess(LinkedList<SyntaxToken> tokens, PreprocessorContext? context = null)
        {
            _issues                      = new List<CodeIssue>();
            _tokens                      = tokens;
            _context                     = context ?? new PreprocessorContext();
            var (includes, defines, ifs) = _context;

            for (_it = _tokens.First; _it != null; _it = _it?.Next)
            {
                SyntaxToken  token = _it.Value;
                SyntaxToken? next  = _it.Next?.Value;

                if (next is null)
                {
                    break;
                }

                switch (token.Kind)
                {
                    case SyntaxKind.DefineDirective:
                        defines.Add(next);

                        break;
                    case SyntaxKind.IncludeDirective:
                        includes.Add(next);

                        break;
                    case SyntaxKind.IfDefDirective:
                        ifs.Push(token);

                        if (defines.Any(t => t.Text == next.Text))
                        {
                            break;
                        }

                        SkipIf();

                        break;
                    case SyntaxKind.IfNDefDirective:
                        ifs.Push(token);

                        if (defines.Count == 0 || defines.Any(t => t.Text != next.Text))
                        {
                            break;
                        }

                        SkipIf();

                        break;
                    case SyntaxKind.UndefDirective:
                        SyntaxToken? define = defines.Find(t => t.Text == next.Text);

                        if (define != null)
                        {
                            defines.Remove(define);

                            break;
                        }

                        _issues.Add(new CodeIssue(IssuesId.UnknownMacrosDefinition, next, next.Text));

                        break;
                    case SyntaxKind.EndIfDirective:
                        if (ifs.Count == 0)
                        {
                            _issues.Add(new CodeIssue(IssuesId.ExtraEndIf, token));

                            break;
                        }

                        ifs.Pop();

                        break;
                    case SyntaxKind.ElseDirective:
                        if (ifs.Count == 0)
                        {
                            _issues.Add(new CodeIssue(IssuesId.UnexpectedElse, token));
                        }

                        continue;
                    default:
                        continue;
                }
            }

            if (ifs.Count <= 0)
            {
                return Context;
            }

            SyntaxToken last = ifs.Last();
            _issues.Add(new CodeIssue(IssuesId.EndIfNotFound, last, last.Text));

            return Context;
        }

        /// <summary>
        ///     Пропускает все токены до первого нахождение #endif или #else.
        /// </summary>
        /// <returns>Возвращает true - если #endif был найден.</returns>
        private void SkipIf()
        {
            while (_it is not null)
            {
                SyntaxToken? token = _it.Next?.Value;

                if (token is null)
                {
                    return;
                }

                if (token.Kind is SyntaxKind.EndIfDirective or SyntaxKind.ElseDirective)
                {
                    return;
                }

                _it = _it.Next;
            }
        }
    }
}
