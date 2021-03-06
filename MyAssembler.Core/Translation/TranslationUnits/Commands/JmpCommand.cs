﻿using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    public class JmpCommand
        : ControlFlowCommand
    {
        public JmpCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        protected override byte[] GetTranslatedBytes()
        {
            int bytesCount = 3;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte("11101001");

            return translatedBytes;
        }

        protected override void UseAddress(TranslationContext context, short address)
        {
            int addrStartPos = 1;
            short jumpValue = CalculateJumpValue(context, address);

            context.InsertAddressValue(addrStartPos, jumpValue);
        }
    }
}
