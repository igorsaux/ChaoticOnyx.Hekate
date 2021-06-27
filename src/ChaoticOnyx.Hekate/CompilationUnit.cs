using System.Collections.Generic;
using System.Collections.Immutable;

namespace ChaoticOnyx.Hekate
{
    /// <summary>
    ///     Результат парсинга.
    /// </summary>
    public sealed record CompilationUnit
    {
        /// <summary>
        ///     Лексер, используемый в этой единице компиляции.
        /// </summary>
        public Lexer Lexer { get; }

        /// <summary>
        ///     Препроцессор, используемый в этой единице компиляции.
        /// </summary>
        public Preprocessor Preprocessor { get; }

        /// <summary>
        ///     Контекст препроцессора на момент конца файла.
        /// </summary>
        public PreprocessorContext Context { get; private set; }

        /// <summary>
        ///     Токены в этой единице компиляции.
        /// </summary>
        public LinkedList<SyntaxToken> Tokens => Lexer.Tokens;

        private CompilationUnit(Lexer lexer, Preprocessor preprocessor)
        {
            Lexer        = lexer;
            Preprocessor = preprocessor;
            Context      = PreprocessorContext.Empty;
        }

        public static CompilationUnit FromSource(string source, Preprocessor? preprocessor = null) => Create(new Lexer(source), preprocessor ?? new Preprocessor());

        public static CompilationUnit FromTokens(IImmutableList<SyntaxToken> tokens, Preprocessor? preprocessor = null) => Create(new Lexer(tokens), preprocessor ?? new Preprocessor());

        public static CompilationUnit FromToken(SyntaxToken token, Preprocessor? preprocessor = null)
        {
            Lexer lexer = new(new List<SyntaxToken>
            {
                token
            }.ToImmutableList());

            return Create(new Lexer(new List<SyntaxToken>
            {
                token
            }.ToImmutableList()), preprocessor ?? new Preprocessor());
        }

        private static CompilationUnit Create(Lexer lexer, Preprocessor preprocessor) => new(lexer, preprocessor);

        /// <summary>
        ///     Производит парсинг и возвращает контекст препроцессора на конец файла.
        /// </summary>
        public void Parse(PreprocessorContext? context = null)
        {
            Lexer.Parse();
            Context = Preprocessor.Preprocess(Tokens, context ?? PreprocessorContext.Empty);
        }

        public IImmutableList<CodeIssue> GetIssues()
        {
            List<CodeIssue> issues = new();
            issues.AddRange(Lexer.Issues);
            issues.AddRange(Preprocessor.Issues);

            return issues.ToImmutableList();
        }

        public string Emit() => Lexer.Emit();
    }
}
