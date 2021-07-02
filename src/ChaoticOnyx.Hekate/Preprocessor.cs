using System.Collections.Generic;
using System.Linq;

namespace ChaoticOnyx.Hekate
{
    /// <summary>
    ///     Препроцессирует набор токенов и возвращает итоговый контекст.
    /// </summary>
    public sealed class Preprocessor
    {
        private LinkedListNode<SyntaxToken>? _it;
        private LinkedList<SyntaxToken>      _tokens = new();

        public List<CodeIssue> Issues { get; private set; } = new();

        /// <summary>
        ///     Текущий контекст препроцессора.
        /// </summary>
        public PreprocessorContext Context { get; private set; } = null!;

        /// <summary>
        ///     Производит препроцессинг токенов.
        /// </summary>
        public PreprocessorContext Preprocess(LinkedList<SyntaxToken> tokens, PreprocessorContext? context = null)
        {
            Issues                       = new List<CodeIssue>();
            _tokens                      = tokens;
            Context                      = context ?? new PreprocessorContext();
            var (includes, defines, ifs) = Context;

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
                        }

                        break;
                    case SyntaxKind.EndIfDirective:
                        if (ifs.Count == 0)
                        {
                            Issues.Add(new CodeIssue(IssuesId.ExtraEndIf, token));

                            break;
                        }

                        ifs.Pop();

                        break;
                    case SyntaxKind.ElseDirective:
                        if (ifs.Count == 0)
                        {
                            Issues.Add(new CodeIssue(IssuesId.UnexpectedElse, token));
                        }

                        continue;
                    case SyntaxKind.WarningDirective:
                        Issues.Add(new CodeIssue(IssuesId.WarningDirective, token, next.Text));

                        continue;
                    case SyntaxKind.ErrorDirective:
                        Issues.Add(new CodeIssue(IssuesId.ErrorDirective, token, next.Text));

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
            Issues.Add(new CodeIssue(IssuesId.EndIfNotFound, last, last.Text));

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
