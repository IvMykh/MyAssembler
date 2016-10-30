using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    public class NotCommand
        : AsmCommand
    {
        public NotCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        private void translateForReg(TranslationContext context, int startPos)
        {
            RegisterType register = context.RegisterHelper.Parse(Tokens[startPos].Value);
            WValue w = context.WValueHelper.WValueForRegister(register);

            int bytesCount = 2;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1111011{0}",
                    context.WValueHelper.WValueToString(w)));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("11010{0}",
                    context.RegisterHelper.RegisterToBitString(register)));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForMem(TranslationContext context, int startPos)
        {
            string identifier = Tokens[startPos].Value;
            IdentifierType idType = context.MemoryManager.GetIdentifierType(identifier);

            if (idType == IdentifierType.Label)
            {
                throw new TranslationErrorException(
                   string.Format("{0}: label identifier is not valid in this context.",
                       identifier));
            }

            int w = (idType == IdentifierType.Byte) ? 0 : 1;
            
            int bytesCount = 4;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1111011{0}",
                    w.ToString()));

            translatedBytes[1] = BitStringHelper.BitStringToByte("00010110");

            context.AddTranslatedUnit(translatedBytes);
        }

        protected override void Translate(TranslationContext context)
        {
            int i = 1;
            if (Tokens[0].Type == TokenType.Identifier)
            {
                // Skip label and ':' tokens.
                ++i;
                ++i;
            }

            switch (OperandsSetType)
            {
                case OperandsSetType.R: translateForReg(context, i); break;
                case OperandsSetType.M: translateForMem(context, i); break;

                default: throw new TranslationErrorException(
                    string.Format("Add: operands set {0} is not supported.",
                        OperandsSetType));
            }
        }
    }
}
