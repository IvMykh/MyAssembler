namespace MyAssembler.Core.LexicalAnalysis
{
    public enum TokenType
    {
        /* Special symbols */
        Comma               = 1,
        OpenSquareBracket   = 2,
        CloseSquareBracket  = 3,
        Plus                = 4,
        Colon               = 5,
        /* Names */
        Directive       = 13,
        Command         = 12,
        Register        = 11,
        Identifier      = 10,
        /* Constants */
        BinConstant     = 6,
        DecConstant     = 7,
        HexConstant     = 8,
        Literal         = 9
    }

    public struct Token
    {
        public TokenType     Type     { get; private set; }
        public string        Value    { get; private set; }
        public TokenPosition Position { get; private set; }

        public Token(TokenType type, string value, TokenPosition position)
            : this()
        {
            Type = type;
            Value = value;
            Position = position;
        }
    }
}
