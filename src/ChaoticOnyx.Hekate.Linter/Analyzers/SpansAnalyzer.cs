using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChaoticOnyx.Hekate.Linter.Analyzers
{
    public sealed class SpansAnalyzer : CodeAnalyzer
    {
        private readonly List<CodeIssue>    _issues  = new();
        private readonly string             _pattern = @"\s*""\s*<\s*span\s+class\s*=\s*['""](?<class>[^'""]*)\s*['""]\s*>(?<content>\s*[^><]*\s*)<\s*\/\s*span\s*>\s*""\s*";
        private          List<SyntaxToken>? _fixedTokens;
        private          bool               _fixMode;

        internal override AnalysisResult Analyze(AnalysisContext context)
        {
            var (style, unit, _) = context;
            _fixMode             = context.TryToFix;
            List<SyntaxToken> tokens = unit.Lexer.Tokens.ToList();

            if (unit.Context.Defines.FirstOrDefault(define => define.Text == "SPAN") == null)
            {
                return new AnalysisResult(ImmutableList<CodeIssue>.Empty, null);
            }

            if (_fixMode)
            {
                _fixedTokens = new List<SyntaxToken>();
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                SyntaxToken token = tokens[i];

                switch (token.Kind)
                {
                    case SyntaxKind.TextLiteral:
                        Match match = Regex.Match(token.Text, _pattern);

                        if (!match.Success)
                        {
                            goto default;
                        }

                        if (!_fixMode || _fixedTokens == null)
                        {
                            _issues.Add(new CodeIssue(Issues.UseSpan, token));
                            goto default;
                        }

                        string classes = match.Groups["class"]
                                              .Value;

                        string content = match.Groups["content"]
                                              .Value;

                        _fixedTokens.Add(SyntaxFactory.Identifier("SPAN")
                                                      .WithLeads(token.Leads.ToArray()));

                        _fixedTokens.Add(SyntaxFactory.OpenParentheses());
                        _fixedTokens.Add(SyntaxFactory.TextLiteral(classes));
                        _fixedTokens.Add(SyntaxFactory.Comma());
                        _fixedTokens.Add(SyntaxFactory.TextLiteral(content));

                        _fixedTokens.Add(SyntaxFactory.CloseParentheses()
                                                      .WithTrails(token.Trails.ToArray()));

                        break;
                    default:
                        _fixedTokens?.Add(token);

                        break;
                }
            }

            CompilationUnit? fixedUnit = _fixedTokens is null
                ? null
                : CompilationUnit.FromTokens(_fixedTokens.ToImmutableList());

            return new AnalysisResult(_issues.ToImmutableList(), fixedUnit);
        }

        internal override void Clear()
        {
            _fixedTokens?.Clear();
            _fixedTokens = null;
            _issues.Clear();
            _fixMode = false;
        }
    }
}
