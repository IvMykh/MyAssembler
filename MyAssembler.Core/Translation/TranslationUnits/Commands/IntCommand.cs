using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.OperandsTypeChecking;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    public class IntCommand
        : AsmCommand
    {
        public IntCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        protected override void TranslateCommand(TranslationContext context, int startPos)
        {
            Constant constant = null;
            context.Checker.CheckIm8(Tokens, startPos, out constant);

            int bytesCount = 2;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte("11001101");
            translatedBytes[1] = constant.Bytes[0];

            context.AddTranslatedUnit(translatedBytes);
        }

        protected override void UseAddress(TranslationContext context, short address)
        { /* Do nothing */}
    }
}
