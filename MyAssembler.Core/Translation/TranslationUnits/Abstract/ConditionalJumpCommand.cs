using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class ConditionalJumpCommand
        : ControlFlowCommand
    {
        public ConditionalJumpCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        protected abstract string SecondBytePlaceholder { get; }

        protected override byte[] GetTranslatedBytes()
        {
            int bytesCount = 4;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte("00001111");
            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("1000{0}",
                    SecondBytePlaceholder));

            return translatedBytes;
        }

        protected override void UseAddress(TranslationContext context, short address)
        {
            int addrStartPos = 2;
            short jumpValue = CalculateJumpValue(context, address);

            context.InsertAddressValue(addrStartPos, jumpValue, true);
        }
    }
}
