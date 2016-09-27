namespace MyAssembler.Core.LexicalAnalysis
{
    public struct TokenDefinition
    {
        public TokenType Type  { get; set; }
        public string    Regex { get; set; }

        public TokenDefinition(TokenType type, string regex)
            : this()
        {
            Type = type;
            Regex = regex;
        }
    }
}
