using System;

namespace ChaoticOnyx.Hekate.Parser
{
    [Flags]
    public enum ParsingModes
    {
        /// <summary>
        ///     Используется только лексер.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Также используется семантический парсер.
        /// </summary>
        WithSemantic = 1 << 0,

        /// <summary>
        ///     Также используется препроцессор.
        /// </summary>
        WithPreprocessor = 1 << 1,

        /// <summary>
        ///     Полный парсинг.
        /// </summary>
        Full = WithSemantic | WithPreprocessor
    }
}
