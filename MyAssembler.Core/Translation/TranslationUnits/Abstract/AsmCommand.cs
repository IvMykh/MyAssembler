using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class AsmCommand
        : AsmTranslationUnit
    {
        public OperandsSetType OperandsSetType { get; protected set; }

        public AsmCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens)
        {
            OperandsSetType = operandsSetType;
        }

        protected void CheckForRegMemMismatch(
            TranslationContext context, RegisterType register, string identifier)
        {
            WValue wForReg = context.WValueHelper.WValueForRegister(register);
            IdentifierType idType = context.MemoryManager.GetIdentifierType(identifier);

            if (idType == IdentifierType.Label)
            {
                throw new TranslationErrorException(
                    string.Format("{0}: label identifier is not valid in this context.",
                        identifier));
            }

            if (idType == IdentifierType.Byte && wForReg == WValue.One ||
                idType == IdentifierType.Word && wForReg == WValue.Zero)
            {
                throw new TranslationErrorException(
                    string.Format("{0} and {1}: operands type mismatch.",
                        register,
                        identifier));
            }
        }
        protected void CheckForRegImMismatch(
            TranslationContext context, RegisterType register, Token constToken)
        {

            WValue w1 = context.WValueHelper.WValueForRegister(register);

            ConstantsParser parser = GetConstsParser(context, constToken.Type);
            byte[] constBytes = parser.Parse(constToken.Value);

            if (w1 == WValue.Zero && constBytes.Length == 2)
            {
                throw new TranslationErrorException(
                    string.Format("{0} and {1}: operands type mismatch.",
                        register,
                        constToken.Value));
            }
        }
        protected void CheckForMemImMismatch(
            TranslationContext context, string identifier, Token constToken)
        {
            IdentifierType idType = context.MemoryManager.GetIdentifierType(identifier);

            ConstantsParser parser = GetConstsParser(context, constToken.Type);
            byte[] constBytes = parser.Parse(constToken.Value);

            if (idType == IdentifierType.Label)
            {
                throw new TranslationErrorException(
                    string.Format("{0}: label identifier is not valid in this context.",
                        identifier));
            }

            if (idType == IdentifierType.Byte && constBytes.Length == 2)
            {
                throw new TranslationErrorException(
                    string.Format("{0} and {1}: operands type mismatch.",
                        identifier,
                        constToken.Value));
            }
        }

        protected ConstantsParser GetConstsParser(TranslationContext context, TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.BinConstant: return context.BinConstsParser;
                case TokenType.DecConstant: return context.DecConstsParser;
                case TokenType.HexConstant: return context.HexConstsParser;

                default: throw new TranslationErrorException(
                    string.Format("{0}: such constant type is not supported.",
                        tokenType));
            }
        }
    }
}
