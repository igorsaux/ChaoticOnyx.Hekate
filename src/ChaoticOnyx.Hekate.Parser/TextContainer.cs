#region

using System.Linq;

#endregion

namespace ChaoticOnyx.Hekate.Parser
{
    internal class TextContainer : TypeContainer<char>
    {
        public string LexemeText
            => new(List.GetRange(Position, Offset - Position)
                       .ToArray());

        public TextContainer(string text) : base(text.ToArray()) { }

        private int _line = 1;

        private int _column = 1;

        /// <summary>
        ///     Возвращает позицию в файле.
        /// </summary>
        public FileLine FilePosition => new(_line, _column);

        public override void   Reset()
        {
            base.Reset();

            _line   = 1;
            _column = 1;
        }

        public override char   Read()
        {
            var @char = base.Read();

            if (@char == '\n')
            {
                _line   += 1;
                _column =  1;
            }
            else
            {
                _column += 1;
            }
            
            return @char;
        }

        public override void   Advance(int    offset = 1)
        {
            var start = offset;
            var len   = start + offset;
            
            for (var i = start; i < len; i++)
            {
                var @char = Peek();

                if (@char == '\n')
                {
                    _line   += 1;
                    _column =  1;
                }
                else
                {
                    _column += 1;
                }

                Offset += 1;
            }
        }
    }
}
