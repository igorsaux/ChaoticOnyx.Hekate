namespace ChaoticOnyx.Hekate.Parser
{
    public sealed record FileLine
    {
        public readonly int Column = 1;
        public readonly int Line   = 1;

        public FileLine(int line, int column)
        {
            Line   = line;
            Column = column;
        }
    }
}
