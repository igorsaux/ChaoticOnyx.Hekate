using System;
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
        public (List<CodeIssue>, PreprocessorContext) Preprocess(LinkedList<SyntaxToken> tokens, PreprocessorContext? context = null)
        {
            bool skipElseBranches = false;
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
                    // TODO: Добавить поддержку многострочников.
                    case SyntaxKind.DefineDirective:
                        if (defines.ContainsKey(next.Text))
                        {
                            Issues.Add(new CodeIssue(IssuesId.VariableAlreadyDefined, next, next.Text));

                            break;
                        }

                        string value = string.Empty;

                        if (!next.HasEndOfLine)
                        {
                            value = _it.Next?.Next?.Value.Text ?? string.Empty;
                        }

                        defines.Add(next.Text, value);

                        break;
                    case SyntaxKind.IncludeDirective:
                        includes.Add(next);

                        break;
                    case SyntaxKind.IfDefDirective:
                        ifs.Push(token);

                        if (defines.ContainsKey(next.Text))
                        {
                            skipElseBranches = true;

                            break;
                        }

                        SkipIf();

                        break;
                    case SyntaxKind.IfNDefDirective:
                        ifs.Push(token);

                        if (defines.Count == 0 || !defines.ContainsKey(next.Text))
                        {
                            skipElseBranches = true;

                            break;
                        }

                        SkipIf();

                        break;
                    case SyntaxKind.UndefDirective:
                        if (defines.ContainsKey(next.Text))
                        {
                            defines.Remove(next.Text);
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
                        if (skipElseBranches)
                        {
                            SkipIf();
                            skipElseBranches = false;

                            break;
                        }

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
                    case SyntaxKind.IfDirective:
                        ifs.Push(token);
                        bool res = ComputeExpression(context!);

                        if (!res)
                        {
                            SkipIf();
                        }

                        continue;
                    case SyntaxKind.ElifDirective:
                        if (skipElseBranches)
                        {
                            SkipIf();
                            skipElseBranches = false;

                            break;
                        }
                        res = ComputeExpression(context!);

                        if (!res)
                        {
                            SkipIf();
                        }

                        continue;
                    default:
                        continue;
                }
            }

            if (ifs.Count <= 0)
            {
                return (Issues, Context);
            }

            SyntaxToken last = ifs.Last();
            Issues.Add(new CodeIssue(IssuesId.EndIfNotFound, last, last.Text));

            return (Issues, Context);
        }

        /// <summary>
        ///     Вычисляет выражение.
        /// </summary>
        /// <param name="context">Контекст препроцессора.</param>
        /// <returns>Результат выражения.</returns>
        private bool ComputeExpression(PreprocessorContext context)
        {
            bool                         result  = false;
            bool                         negated = false;
            LinkedListNode<SyntaxToken>? proc;
            LinkedListNode<SyntaxToken>? token     = _it?.Next;
            LinkedListNode<SyntaxToken>? nextToken = _it?.Next?.Next;

            if (token is null)
            {
                Issues.Add(new CodeIssue(IssuesId.ExpectedProc, _it?.Value!));

                return false;
            }

            if (token.Value.Kind == SyntaxKind.Exclamation)
            {
                negated = true;
                proc    = nextToken;
            }
            else
            {
                proc = token;
            }

            LinkedListNode<SyntaxToken>? parent = proc.Next;
            LinkedListNode<SyntaxToken>? value  = parent?.Next;

            if (value?.Value.Kind is not (SyntaxKind.Identifier or SyntaxKind.NumericalLiteral))
            {
                Issues.Add(new CodeIssue(IssuesId.ExpectedValue, proc.Value));

                return false;
            }

            switch (proc.Value.Text)
            {
                case "defined":
                    result = ComputeDefined(value, context);

                    break;
                default:
                    value = proc;
                    proc  = proc.Next;
                    var rvalue = proc?.Next;

                    return ComputeOperator(value, proc!, rvalue!, context);
            }

            if (negated)
            {
                return !result;
            }

            return result;
        }

        private bool ComputeOperator(LinkedListNode<SyntaxToken> leftToken, LinkedListNode<SyntaxToken> operatorToken, LinkedListNode<SyntaxToken> rightToken, PreprocessorContext context)
        {
            int  lvalue = 0;
            int  rvalue = 0;
            bool result = false;

            if (leftToken.Value.Kind is SyntaxKind.Identifier)
            {
                bool hasLValue = context.Defines.ContainsKey(leftToken.Value.Text);

                if (!hasLValue)
                {
                    Issues.Add(new CodeIssue(IssuesId.UnknownVariable, leftToken.Value, leftToken.Value.Text));

                    return false;
                }

                result = int.TryParse(context.Defines[leftToken.Value.Text], out lvalue);
            }
            else
            {
                result = int.TryParse(leftToken.Value.Text, out lvalue);
            }

            if (!result)
            {
                Issues.Add(new CodeIssue(IssuesId.CantCompareNotNumericalValues, leftToken.Value));

                return false;
            }

            if (rightToken.Value.Kind is SyntaxKind.Identifier)
            {
                bool hasRValue = context.Defines.ContainsKey(rightToken.Value.Text);

                if (!hasRValue)
                {
                    Issues.Add(new CodeIssue(IssuesId.UnknownVariable, rightToken.Value, rightToken.Value.Text));

                    return false;
                }

                result = int.TryParse(context.Defines[rightToken.Value.Text], out rvalue);
            }
            else
            {
                result = int.TryParse(rightToken.Value.Text, out rvalue);
            }

            if (!result)
            {
                Issues.Add(new CodeIssue(IssuesId.CantCompareNotNumericalValues, rightToken.Value));

                return false;
            }

            switch (operatorToken.Value.Text)
            {
                case "==":
                    return lvalue == rvalue;
                case "!=":
                    return lvalue != rvalue;
                case ">":
                    return lvalue > rvalue;
                case ">=":
                    return lvalue >= rvalue;
                case "<":
                    return lvalue < rvalue;
                case "<=":
                    return lvalue <= rvalue;
                default:
                    Issues.Add(new CodeIssue(IssuesId.InvalidOperator, operatorToken.Value, operatorToken.Value.Text));

                    return false;
            }
        }

        /// <summary>
        ///     Вычисляет функцию defined(X), где X - переменная.
        /// </summary>
        /// <param name="variable">X.</param>
        /// <param name="context">Контекст препроцессора.</param>
        /// <returns>true - если X уже был определён, иначе - false.</returns>
        private static bool ComputeDefined(LinkedListNode<SyntaxToken> variable, PreprocessorContext context)
        {
            return context.Defines.ContainsKey(variable.Value.Text);
        }

        /// <summary>
        ///     Пропускает все токены до первого нахождение #endif, #ifdef, #ifndef или #else.
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

                if (token.Kind is SyntaxKind.EndIfDirective or SyntaxKind.ElseDirective or SyntaxKind.IfDefDirective or SyntaxKind.IfNDefDirective or SyntaxKind.IfDirective or SyntaxKind.ElifDirective)
                {
                    return;
                }

                _it = _it.Next;
            }
        }
    }
}
