#region

using System;

#endregion

namespace ChaoticOnyx.Hekate
{
    /// <summary>
    ///     Контейнер для по-элементного считывания содержимого.
    /// </summary>
    internal sealed class TextContainer
    {
        /// <summary>
        ///     Коллекция контейнера.
        /// </summary>
        private readonly ReadOnlyMemory<char> _buffer;

        private int _offsetColumn   = 1;
        private int _offsetLine     = 1;
        private int _positionColumn = 1;

        private int _positionLine = 1;

        public ReadOnlySpan<char> LexemeText
            => _buffer.Slice(Position, Offset - Position)
                      .Span;

        /// <summary>
        ///     Длина всей коллекции.
        /// </summary>
        public int Length { get; }

        /// <summary>
        ///     Отступ от начала коллекции.
        /// </summary>
        public int Offset { get; private set; }

        /// <summary>
        ///     Последняя позиция от начала коллекции.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        ///     Если текущий отступ от начала коллекции в конце и дальше возвращает true.
        /// </summary>
        public bool IsEnd => Offset >= Length;

        /// <summary>
        ///     Возвращает текущую позицию в файле.
        /// </summary>
        public FileLine OffsetFilePosition => new(_offsetLine, _offsetColumn);

        public FileLine LexemeFilePosition => new(_positionLine, _positionColumn);

        public TextContainer(ReadOnlyMemory<char> text)
        {
            _buffer = text;
            Length  = _buffer.Length;
        }

        /// <summary>
        ///     Устанавливает отступ в начало коллекции.
        /// </summary>
        public void Reset()
        {
            Offset = 0;
            Start();
            _offsetLine     = 1;
            _offsetColumn   = 1;
            _positionLine   = 1;
            _positionColumn = 1;
        }

        /// <summary>
        ///     Устанавливает позицию на текущий отступ.
        /// </summary>
        public void Start()
        {
            Position        = Offset;
            _positionLine   = _offsetLine;
            _positionColumn = _offsetColumn;
        }

        /// <summary>
        ///     Возвращает элемент на текущем отступе и увеличивает отступ на единицу.
        /// </summary>
        /// <returns></returns>
        public ReadOnlySpan<char> Read()
        {
            ReadOnlySpan<char> span = _buffer.Slice(Offset++, 1)
                                             .Span;

            if (span[0] == '\n')
            {
                _offsetLine   += 1;
                _offsetColumn =  1;
            }
            else
            {
                _offsetColumn += 1;
            }

            return span;
        }

        /// <summary>
        ///     Возвращает элемент на определённом количестве шагов от текущего отступа и не увеличивает индекс.
        /// </summary>
        /// <param name="offset">Количество шагов от текущего отступа.</param>
        /// <returns>Возвращает null если указанный отступ выходит за конец коллекции.</returns>
        public ReadOnlySpan<char> Peek(int offset = 1)
        {
            int result = Offset + offset - 1;

            return result >= Length
                ? default(ReadOnlySpan<char>)
                : _buffer.Slice(result, 1)
                         .Span;
        }

        /// <summary>
        ///     Передвигает отступ на указанное количество шагов.
        /// </summary>
        /// <param name="offset">Количество шагов</param>
        public void Advance(int offset = 1)
        {
            int start = offset;
            int len   = start + offset;

            for (int i = start; i < len; i++)
            {
                ReadOnlySpan<char> span = Peek();

                if (span[0] == '\n')
                {
                    _offsetLine   += 1;
                    _offsetColumn =  1;
                }
                else
                {
                    _offsetColumn += 1;
                }

                Offset += 1;
            }
        }
    }
}
