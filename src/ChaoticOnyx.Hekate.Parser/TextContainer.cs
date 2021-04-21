#region

using System.Linq;

#endregion

namespace ChaoticOnyx.Hekate.Parser
{
    internal class TextContainer : TypeContainer<char>
    {
        public readonly int TabWidth;
        private         int _offsetColumn = 1;

        private int _offsetLine     = 1;
        private int _positionColumn = 1;

        private int _positionLine = 1;

        public string LexemeText
            => new(List.ToList()
                       .GetRange(Position, Offset - Position)
                       .ToArray());

        /// <summary>
        ///     Возвращает текущую позицию в файле.
        /// </summary>
        public FileLine OffsetFilePosition => new(_offsetLine, _offsetColumn);

        public FileLine LexemeFilePosition => new(_positionLine, _positionColumn);

        public TextContainer(string text, int tabWidth) : base(text.ToArray()) => TabWidth = tabWidth;

        public override void Reset()
        {
            base.Reset();
            _offsetLine     = 1;
            _offsetColumn   = 1;
            _positionLine   = 1;
            _positionColumn = 1;
        }

        public override void Start()
        {
            base.Start();
            _positionLine   = _offsetLine;
            _positionColumn = _offsetColumn;
        }

        public override char Read()
        {
            char @char = base.Read();

            if (@char == '\n')
            {
                _offsetLine   += 1;
                _offsetColumn =  1;
            }
            else if (@char == '\t')
            {
                _offsetColumn += TabWidth;
            }
            else
            {
                _offsetColumn += 1;
            }

            return @char;
        }

        public override void Advance(int offset = 1)
        {
            int start = offset;
            int len   = start + offset;

            for (int i = start; i < len; i++)
            {
                char @char = Peek();

                if (@char == '\n')
                {
                    _offsetLine   += 1;
                    _offsetColumn =  1;
                }
                else if (@char == '\t')
                {
                    _offsetColumn += TabWidth;
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
