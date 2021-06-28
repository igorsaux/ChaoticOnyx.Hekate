using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ChaoticOnyx.Hekate
{
    /// <summary>
    ///     Контекст препроцессора.
    /// </summary>
    public sealed record PreprocessorContext
    {
        public List<SyntaxToken>  Includes { get; init; }
        public List<SyntaxToken>  Defines  { get; init; }
        public Stack<SyntaxToken> Ifs      { get; init; }

        /// <summary>
        ///     Создаёт новый контекст.
        /// </summary>
        /// <param name="includes"></param>
        /// <param name="defines"></param>
        public PreprocessorContext(List<SyntaxToken> includes, List<SyntaxToken> defines, Stack<SyntaxToken> ifs)
        {
            Includes = includes;
            Defines  = defines;
            Ifs      = ifs;
        }

        /// <summary>
        ///     Создаёт пустой контект. Эквивалентно PreprocessorContext.Empty.
        /// </summary>
        public PreprocessorContext()
        {
            Includes = new List<SyntaxToken>();
            Defines  = new List<SyntaxToken>();
            Ifs      = new Stack<SyntaxToken>();
        }

        public void Deconstruct(out List<SyntaxToken> includes, out List<SyntaxToken> defines, out Stack<SyntaxToken> ifs)
        {
            includes = Includes;
            defines  = Defines;
            ifs      = Ifs;
        }
    }
}
