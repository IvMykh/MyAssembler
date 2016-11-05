using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.OperandsTypeChecking;
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

        protected override byte[] GetTranslatedBytesForRegIm(Register reg, Constant constant)
        {
            // 2 or 3
            int bytesCount = 2 + reg.W;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1011{0}{1}", reg.W, reg));

            translatedBytes[1] = constant.Bytes[0];

            if (constant.Bytes.Length == 2)
            {
                translatedBytes[2] = constant.Bytes[1];
            }

            return translatedBytes;
        }
        protected override byte[] GetTranslatedBytesForMemIm(Identifier identifier, Constant constant)
        {
            int bytesCount = 6;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1100011{0}", identifier.W));

            // mod reg r/m
            translatedBytes[1] = BitStringHelper.BitStringToByte("00000110");

            // Address.
            translatedBytes[2] = 0x00;
            translatedBytes[3] = 0x00;

            translatedBytes[4] = constant.Bytes[0];

            if (identifier.Type == IdentifierType.Byte)
            {
                translatedBytes[5] = 0x90; // NOP;
            }
            else if (constant.Bytes.Length == 2)
            {
                translatedBytes[5] = constant.Bytes[1];
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
