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
        public IImmutableList<SyntaxToken> Includes { get; init; }
        public IImmutableList<SyntaxToken> Defines  { get; init; }

        /// <summary>
        ///     Возвращает пустой контекст.
        /// </summary>
        public static PreprocessorContext Empty => new();

        /// <summary>
        ///     Создаёт новый контекст.
        /// </summary>
        /// <param name="includes"></param>
        /// <param name="defines"></param>
        public PreprocessorContext(IImmutableList<SyntaxToken> includes, IImmutableList<SyntaxToken> defines)
        {
            Includes = includes;
            Defines  = defines;
        }

        /// <summary>
        ///     Создаёт пустой контект. Эквивалентно PreprocessorContext.Empty.
        /// </summary>
        public PreprocessorContext()
        {
            Includes = ImmutableList<SyntaxToken>.Empty;
            Defines  = ImmutableList<SyntaxToken>.Empty;
        }

        public void Deconstruct(out List<SyntaxToken> includes, out List<SyntaxToken> defines)
        {
            includes = Includes.ToList();
            defines  = Defines.ToList();
        }
    }
}
