namespace ChaoticOnyx.Hekate.Parser
{
    /// <summary>
    ///     Минимальная единица компиляции.
    /// </summary>
    public class CompilationUnit
    {
        private readonly string _source;

        /// <summary>
        ///     Лексер для данной единицы компиляции.
        /// </summary>
        public Lexer Lexer
        {
            get;
        }

        /// <summary>
        ///     Создание новой единцы компиляции из текста.
        /// </summary>
        /// <param name="source">Исходный код данной единицы.</param>
        public CompilationUnit(string source)
        {
            Lexer   = new(source);
            _source = source;
        }

        /// <summary>
        ///     Создание новой единицы компиляции из набора токенов.
        /// </summary>
        /// <param name="tokens">Набор токенов.</param>
        public CompilationUnit(params SyntaxToken[] tokens)
        {
            Lexer   = new(tokens);
            _source = Lexer.Emit();
        }

        /// <summary>
        ///     Осуществление парсинга данной единцы компиляции.
        /// </summary>
        public void Parse()
        {
            Lexer.Parse();
        }

        /// <summary>
        ///     Осуществляет превращение в текст.
        /// </summary>
        public string Emit()
        {
            return Lexer.Emit();
        }
    }
}
