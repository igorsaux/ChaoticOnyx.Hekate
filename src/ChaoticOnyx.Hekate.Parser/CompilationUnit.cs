using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ChaoticOnyx.Hekate.Parser
{
    /// <summary>
    ///     Результат парсинга.
    /// </summary>
    public sealed record CompilationUnit
    {
        public readonly Lexer        Lexer;
        public readonly ParsingModes Modes;
        public readonly Preprocessor Preprocessor;

        private CompilationUnit(Lexer lexer, Preprocessor preprocessor, ParsingModes modes)
        {
            Lexer        = lexer;
            Preprocessor = preprocessor;
            Modes        = modes;
        }

        public static CompilationUnit FromSource(string source, int tabWidth = 4, ParsingModes modes = ParsingModes.Full)
        {
            Lexer lexer = new(source, tabWidth);
            lexer.Parse();

            return Create(lexer, modes);
        }

        public static CompilationUnit FromTokens(IList<SyntaxToken> tokens, int tabWidth = 4, ParsingModes modes = ParsingModes.Full)
        {
            Lexer lexer = new(tokens, tabWidth);

            return Create(lexer, modes);
        }

        public static CompilationUnit FromToken(SyntaxToken token, int tabWidth = 4, ParsingModes modes = ParsingModes.Full)
        {
            Lexer lexer = new(new Collection<SyntaxToken>
            {
                token
            }, tabWidth);

            return Create(lexer, modes);
        }

        private static CompilationUnit Create(Lexer lexer, ParsingModes modes)
        {
            Preprocessor preprocessor = modes.HasFlag(ParsingModes.WithPreprocessor)
                ? Preprocessor.WithTokens(lexer.Tokens)
                : Preprocessor.WithoutTokens();

            preprocessor.Preprocess();

            return new CompilationUnit(lexer, preprocessor, modes);
        }

        public ICollection<CodeIssue> GetIssues()
        {
            List<CodeIssue> issues = new();
            issues.AddRange(Lexer.Issues);
            issues.AddRange(Preprocessor.Issues);

            return issues;
        }

        public string Emit() => Lexer.Emit();
    }
}
