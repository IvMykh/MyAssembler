using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.OperandsTypeChecking;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class SingleOperandCommand
        : AsmCommand
    {
        public SingleOperandCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        protected abstract string OperationCodeFormat { get; }
        protected abstract string AddressingByteFormat { get; }

        private void translateForReg(TranslationContext context, int startPos)
        {
            Register reg = null;
            context.Checker.CheckReg(Tokens, startPos, out reg);

            int bytesCount = 2;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format(OperationCodeFormat, reg.W));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format(AddressingByteFormat, "11", reg));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForMem(TranslationContext context, int startPos)
        {
            Identifier identifier = null;
            context.Checker.CheckMem(Tokens, startPos, out identifier);

            int bytesCount = 4;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format(OperationCodeFormat,
                    identifier.W));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format(AddressingByteFormat, "00", "110"));

            context.AddTranslatedUnit(translatedBytes);
        }

        protected override void TranslateCommand(TranslationContext context, int startPos)
        {
            switch (OperandsSetType)
            {
                case OperandsSetType.R: translateForReg(context, startPos); break;
                case OperandsSetType.M: translateForMem(context, startPos); break;

                default: 
                    ThrowForUnsupportedOST(OperandsSetType, Tokens[startPos].Position); break;
            }
        }
    }
}
