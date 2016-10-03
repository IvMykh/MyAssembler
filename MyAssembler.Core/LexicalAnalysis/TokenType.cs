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
        QuestionMark        = 6,
        /* Names */
        Directive           = 14,
        Command             = 13,
        Register            = 12,
        Identifier          = 11,
        /* Constants */
        BinConstant         = 7,
        DecConstant         = 8,
        HexConstant         = 9,
        Literal             = 10
    }
}
