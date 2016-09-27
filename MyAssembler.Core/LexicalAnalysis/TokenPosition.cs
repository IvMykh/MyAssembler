namespace MyAssembler.Core.LexicalAnalysis
{
    public struct TokenPosition
    {
        public int Line       { get; private set; }
        public int StartIndex { get; private set; }

        public TokenPosition(int line, int startIndex)
            : this()
        {
            Line = line;
            StartIndex = startIndex;
        }
    }
}
