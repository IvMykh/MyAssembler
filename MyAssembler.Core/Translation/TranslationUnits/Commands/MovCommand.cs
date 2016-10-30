using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    public class MovCommand
        : FiveCasesCommand
    {
        public MovCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        protected override byte[] GetTranslatedBytesForRegIm(
            TranslationContext context, RegisterType register, byte[] constBytes)
        {
            WValue w1 = context.WValueHelper.WValueForRegister(register);

            // 2 or 3
            int bytesCount = 2 + context.WValueHelper.WValueToByte(w1);
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1011{0}{1}",
                    context.WValueHelper.WValueToString(w1),
                    context.RegisterHelper.RegisterToBitString(register)));

            translatedBytes[1] = constBytes[0];

            if (constBytes.Length == 2)
            {
                translatedBytes[2] = constBytes[1];
            }

            return translatedBytes;
        }

        protected override byte[] GetTranslatedBytesForMemIm(
            TranslationContext context, string identifier, byte[] constBytes)
        {
            IdentifierType idType = context.MemoryManager.GetIdentifierType(identifier);

            int bytesCount = 6;
            byte[] translatedBytes = new byte[bytesCount];

            int wForIdentifier = (idType == IdentifierType.Byte) ? 0 : 1;

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1100011{0}",
                    wForIdentifier.ToString()));

            // mod reg r/m
            translatedBytes[1] = BitStringHelper.BitStringToByte("00000110");

            // Address.
            translatedBytes[2] = 0x00;
            translatedBytes[3] = 0x00;

            translatedBytes[4] = constBytes[0];

            if (idType == IdentifierType.Byte)
            {
                translatedBytes[5] = 0x90; // NOP;
            }
            else if (constBytes.Length == 2)
            {
                translatedBytes[5] = constBytes[1];
            }

            return translatedBytes;
        }

        protected override string RegRegFormat
        {
            get {
                return "1000101{0}"; 
            }
        }
        protected override string RegMemFormat
        {
            get {
                return "1000101{0}"; 
            }
        }
        protected override string MemRegFormat
        {
            get { 
                return "1000100{0}"; 
            }
        }
    }
}
