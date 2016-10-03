namespace MyAssembler.Core.LexicalAnalysis
{
    public struct Token
    {
        public TokenType     Type     { get; private set; }
        public string        Value    { get; private set; }
        public TokenPosition Position { get; private set; }

        public Token(TokenType type, string value, TokenPosition position = default(TokenPosition))
            : this()
        {
            Type        = type;
            Value       = value;
            Position    = position;
        }
    }
}
