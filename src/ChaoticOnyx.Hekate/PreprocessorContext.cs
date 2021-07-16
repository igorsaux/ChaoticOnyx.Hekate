using System.Collections.Generic;

namespace ChaoticOnyx.Hekate
{
    /// <summary>
    ///     Контекст препроцессора.
    /// </summary>
    public sealed record PreprocessorContext
    {
        public List<SyntaxToken>  Includes { get; init; }
        public Dictionary<string, string>      Defines  { get; init; }
        public Stack<SyntaxToken> Ifs      { get; init; }

        /// <summary>
        ///     Создаёт новый контекст.
        /// </summary>
        /// <param name="includes"></param>
        /// <param name="defines"></param>
        /// <param name="ifs"></param>
        public PreprocessorContext(List<SyntaxToken> includes, Dictionary<string, string> defines, Stack<SyntaxToken> ifs)
        {
            Includes = includes;
            Defines  = defines;
            Ifs      = ifs;
        }

        /// <summary>
        ///     Создаёт пустой контект.
        /// </summary>
        public PreprocessorContext()
        {
            Includes = new List<SyntaxToken>();
            Defines  = new Dictionary<string, string>();
            Ifs      = new Stack<SyntaxToken>();
        }

        public void Deconstruct(out List<SyntaxToken> includes, out Dictionary<string, string> defines, out Stack<SyntaxToken> ifs)
        {
            includes = Includes;
            defines  = Defines;
            ifs      = Ifs;
        }
    }
}
