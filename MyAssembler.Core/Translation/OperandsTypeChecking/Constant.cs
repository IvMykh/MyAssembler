using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Properties;
using MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants;

namespace MyAssembler.Core.Translation.OperandsTypeChecking
{
    public class Constant
        : Operand
    {
        // Statics.
        private static BinConstantsParser BinParser     = new BinConstantsParser();
        private static DecConstantsParser DecParser     = new DecConstantsParser();
        private static HexConstantsParser HexParser     = new HexConstantsParser();
        private static LiteralParser      LiteralParser = new LiteralParser();

        // Instance.
        public string Value { get; private set; }
        public TokenType Type { get; private set; }
        public byte[] Bytes { get; private set; }

        public Constant(string valueString, TokenType type)
        {
            Value = valueString;
            Type = type;
            Bytes = getParserFor(Type).Parse(Value);

            W = (Bytes.Length == 1) ? WValueStore.ZERO : WValueStore.ONE;
        }

        private ConstantsParser getParserFor(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.BinConstant: return BinParser;
                case TokenType.DecConstant: return DecParser;
                case TokenType.HexConstant: return HexParser;
                case TokenType.Literal:     return LiteralParser;

                default: throw new DesignErrorException(
                    string.Format(Resources.ConstTypeNotSupportedMsgFormat, tokenType));
            }
        }
    }
}
