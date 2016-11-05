using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;

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
    }
}
