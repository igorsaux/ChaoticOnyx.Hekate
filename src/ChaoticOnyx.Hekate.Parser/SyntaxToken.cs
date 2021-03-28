#region

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace ChaoticOnyx.Hekate.Parser
{
    /// <summary>
    ///     Синтаксический токен.
    /// </summary>
    public class SyntaxToken
    {
        private readonly List<SyntaxToken> _leadTokens  = new();
        private readonly List<SyntaxToken> _trailTokens = new();

        /// <summary>
        ///     Тип токена.
        /// </summary>
        public SyntaxKind Kind
        {
            get;
            set;
        }

        /// <summary>
        ///     Содержимое токена.
        /// </summary>
        public string Text
        {
            get;
        }

        /// <summary>
        ///     Длина содержимого токена с учётом хвостовых и лидирующих токенов.
        /// </summary>
        public int FullLength => GetFullLength();

        /// <summary>
        ///     Длина содержимого токена.
        /// </summary>
        public int Length => Text.Length;

        /// <summary>
        ///     Абсолютное расположение токена в тексте.
        /// </summary>
        public int Position
        {
            get;
        }

        /// <summary>
        ///     Возвращает true если имеет в хвостовых токенах конец линии.
        /// </summary>
        public bool HasEndOfLine => _trailTokens.Count(t => t.Kind == SyntaxKind.EndOfLine) > 0;

        /// <summary>
        ///     Ведущие токены.
        /// </summary>
        public ReadOnlyCollection<SyntaxToken> Leads => _leadTokens.AsReadOnly();

        /// <summary>
        ///     Хвостовые токены.
        /// </summary>
        public ReadOnlyCollection<SyntaxToken> Trails => _trailTokens.AsReadOnly();

        /// <summary>
        ///     Создание нового токена.
        /// </summary>
        /// <param name="kind">Тип токена.</param>
        /// <param name="text">Содержимое токена.</param>
        /// <param name="position">Позиция токена.</param>
        public SyntaxToken(SyntaxKind kind, string text, int position) : this(kind, text)
        {
            Position = position;
        }

        /// <summary>
        ///     Создание нового токена.
        /// </summary>
        /// <param name="kind">Тип токена.</param>
        /// <param name="text">Содержимое токена.</param>
        public SyntaxToken(SyntaxKind kind, string text)
        {
            Kind = kind;
            Text = text;
        }

        /// <summary>
        ///     Создание нового токена.
        /// </summary>
        /// <param name="kind">Тип токена.</param>
        public SyntaxToken(SyntaxKind kind) : this(kind, string.Empty) { }

        /// <summary>
        ///     Создание нового токена.
        /// </summary>
        /// <param name="text">Содержимое токена.</param>
        public SyntaxToken(string text) : this(SyntaxKind.Unknown, text) { }

        /// <summary>
        ///     Получение длиный токена с учетом ведущих и хвостовых токенов.
        /// </summary>
        /// <returns>Длина.</returns>
        private int GetFullLength()
        {
            var result = Text.Length;
            result += Leads.Sum(token => token.FullLength);
            result += Trails.Sum(token => token.FullLength);

            return result;
        }

        /// <summary>
        ///     Добавить лидирующих токенов.
        /// </summary>
        /// <param name="tokens"></param>
        public void AddLeadTokens(params SyntaxToken[] tokens)
        {
            _leadTokens.AddRange(tokens);
        }

        /// <summary>
        ///     Добавить хвостовых токенов.
        /// </summary>
        /// <param name="tokens"></param>
        public void AddTrailTokens(params SyntaxToken[] tokens)
        {
            _trailTokens.AddRange(tokens);
        }

        public override string ToString()
        {
            return $"{Kind} - {{{Text}}}";
        }
    }
}
