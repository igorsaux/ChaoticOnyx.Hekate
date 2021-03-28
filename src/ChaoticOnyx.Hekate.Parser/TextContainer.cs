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
    }
}
