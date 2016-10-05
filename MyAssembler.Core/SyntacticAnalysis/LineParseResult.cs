using MyAssembler.Core.LexicalAnalysis;

namespace MyAssembler.Core.SyntacticAnalysis
{
    public struct LineParseResult
    {
        public TokenPosition Position { get; set; }
        public OperandsSetType OperandsSetType { get; set; }
        public string ErrorMsgFormat { get; set; }

        public LineParseResult(
            TokenPosition position, OperandsSetType operandsSetType, string errorMsgFormat = null)
            : this()
        {
            Position = position;
            OperandsSetType = operandsSetType;
            ErrorMsgFormat = errorMsgFormat;
        }
    }
}
