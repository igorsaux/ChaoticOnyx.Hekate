#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ChaoticOnyx.Hekate
{
    /// <summary>
    ///     Синтаксический токен.
    /// </summary>
    public class SyntaxToken
    {
        /// <summary>
        ///     Тип токена.
        /// </summary>
        public SyntaxKind Kind { get; set; }

        /// <summary>
        ///     Содержимое токена.
        /// </summary>
        public string Text { get; }

        public string FullText => GetFullText();

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
        public int Position { get; }

        /// <summary>
        ///     Возвращает относительное расположение токена в тексте.
        /// </summary>
        public FileLine FilePosition { get; } = new(1, 1);

        /// <summary>
        ///     Возвращает true если имеет в хвостовых токенах конец линии.
        /// </summary>
        public bool HasEndOfLine => Trails.Any(t => t.Kind == SyntaxKind.EndOfLine);

        /// <summary>
        ///     Ведущие токены.
        /// </summary>
        public LinkedList<SyntaxToken> Leads { get; } = new();

        /// <summary>
        ///     Хвостовые токены.
        /// </summary>
        public LinkedList<SyntaxToken> Trails { get; } = new();

        /// <summary>
        ///     Создание нового токена.
        /// </summary>
        /// <param name="kind">Тип токена.</param>
        /// <param name="span">Содержимое токена.</param>
        /// <param name="position">Позиция токена.</param>
        public SyntaxToken(SyntaxKind kind, ReadOnlySpan<char> span, int position) : this(kind, span) => Position = position;

        public SyntaxToken(SyntaxKind kind, ReadOnlySpan<char> span, int position, FileLine filePosition) : this(kind, span, position) => FilePosition = filePosition;

        /// <summary>
        ///     Создание нового токена.
        /// </summary>
        /// <param name="kind">Тип токена.</param>
        /// <param name="span">Содержимое токена.</param>
        public SyntaxToken(SyntaxKind kind, ReadOnlySpan<char> span)
        {
            Kind = kind;
            Text = span.ToString();
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
            int result = Text.Length;
            result += Leads.Sum(token => token.FullLength);
            result += Trails.Sum(token => token.FullLength);

            return result;
        }

        /// <summary>
        ///     Добавить лидирующих токенов.
        /// </summary>
        public void AddLeadTokens(params SyntaxToken[] tokens)
        {
            foreach (SyntaxToken token in tokens)
            {
                Leads.AddLast(token);
            }
        }

        /// <summary>
        ///     Добавить хвостовых токенов.
        /// </summary>
        public void AddTrailTokens(params SyntaxToken[] tokens)
        {
            foreach (SyntaxToken token in tokens)
            {
                Trails.AddLast(token);
            }
        }

        /// <summary>
        ///     Выводит текст токена вместе с ведущими и хвостовыми токенами.
        /// </summary>
        public string GetFullText()
        {
            StringBuilder builder = new();

            foreach (SyntaxToken lead in Leads)
            {
                builder.Append(lead.FullText);
            }

            builder.Append(Text);

            foreach (SyntaxToken trail in Trails)
            {
                builder.Append(trail.FullText);
            }

            return builder.ToString();
        }

        public override string ToString() => $"{Kind} - {{{Text}}}";
    }
}
