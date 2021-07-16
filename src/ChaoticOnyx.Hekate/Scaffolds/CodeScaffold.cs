using System.Collections.Generic;

namespace ChaoticOnyx.Hekate.Scaffolds
{
    /// <summary>
    ///     Позволяет конвертировать код из одного представления в другое.
    /// </summary>
    public abstract class CodeScaffold<T>
    {
        /// <summary>
        ///     Лексер, используемый этим классом.
        /// </summary>
        public Lexer Lexer { get; }

        protected CodeScaffold(Lexer lexer) => Lexer = lexer;

        public abstract T GetResult();
    }
}
