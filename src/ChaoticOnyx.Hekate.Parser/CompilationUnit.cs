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
        ///     Создание новой единцы компиляции.
        /// </summary>
        /// <param name="source">Исходный код данной единицы.</param>
        public CompilationUnit(string source)
        {
            Lexer   = new(source);
            _source = source;
        }

        /// <summary>
        ///     Осуществление парсинга данной единцы компиляции.
        /// </summary>
        public void Parse()
        {
            Lexer.Parse();
        }
    }
}
