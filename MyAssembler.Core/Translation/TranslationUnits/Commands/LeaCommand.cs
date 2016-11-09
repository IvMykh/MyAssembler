using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.OperandsTypeChecking;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    public class LeaCommand
        : AsmCommand
    {
        public LeaCommand(List<Token> tokens, OperandsSetType ost)
            : base(tokens, ost)
        {
        }

        protected override void TranslateCommand(TranslationContext context, int startPos)
        {
            Register register = null;
            Identifier identifier = null;
            context.Checker.CheckRegMemAddress(Tokens, startPos, out register, out identifier);

            int bytesCount = 4;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte("10001101");
            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("{0}{1}{2}", "00", register, "110"));

            context.AddTranslatedUnit(translatedBytes);
        }

        protected override void UseAddress(TranslationContext context, short address)
        {
            int addrStartPos = 2;
            context.InsertAddressValue(addrStartPos, address);
        }
    }
}
