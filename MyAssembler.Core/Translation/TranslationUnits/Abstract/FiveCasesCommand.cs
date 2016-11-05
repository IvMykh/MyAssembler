using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.OperandsTypeChecking;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class FiveCasesCommand
        : AsmCommand
    {
        public FiveCasesCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        protected abstract string RegRegFormat  { get; }
        protected abstract string RegMemFormat  { get; }
        protected abstract string MemRegFormat  { get; }
        protected virtual string  RegImFormat   { get { return null; } }
        protected virtual string  MemImFormat   { get { return null; } }

        protected virtual string  RegFieldForIm { get { return null; } }

        protected virtual byte[] GetTranslatedBytesForRegIm(Register reg, Constant constant)
        {
            int bytesCount = (reg.W == WValueStore.ZERO) ? 3 : 2 + constant.Bytes.Length;
            byte[] translatedBytes = new byte[bytesCount];

            int s = (reg.W == WValueStore.ONE && constant.Bytes.Length == 1) ? 1 : 0;

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                    string.Format(RegImFormat, s, reg.W));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                    string.Format("11{0}{1}", RegFieldForIm, reg));

            translatedBytes[2] = constant.Bytes[0];

            if (translatedBytes.Length == 4)
            {
                translatedBytes[3] = constant.Bytes[1];
            }

            return translatedBytes;
        }
        protected virtual byte[] GetTranslatedBytesForMemIm(Identifier identifier, Constant constant)
        {
            int bytesCount = (identifier.Type == IdentifierType.Byte) ? 5 : 4 + constant.Bytes.Length;
            byte[] translatedBytes = new byte[bytesCount];

            int s = (identifier.Type == IdentifierType.Word && constant.Bytes.Length == 1) ? 
                1 : 0;

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                    string.Format(MemImFormat, s, identifier.W));

            // mod reg r/m
            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("00{0}110", RegFieldForIm));

            // Address.
            translatedBytes[2] = 0x00;
            translatedBytes[3] = 0x00;

            translatedBytes[4] = constant.Bytes[0];

            if (translatedBytes.Length == 6)
            {
                translatedBytes[5] = constant.Bytes[1];
            }

            return translatedBytes;
        }
        
        private void translateForRegReg(TranslationContext context, int startPos)
        {
            Register reg1 = null;
            Register reg2 = null;
            context.Checker.CheckRegReg(Tokens, startPos, out reg1, out reg2);

            int bytesCount = 2;
            var translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format(RegRegFormat,
                    reg1.W));

            // mod reg r/m
            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("{0}{1}{2}", "11", reg1, reg2));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForRegMem(TranslationContext context, int startPos)
        {
            Register reg = null;
            Identifier memCell = null;
            context.Checker.CheckRegMem(Tokens, startPos, out reg, out memCell);

            int bytesCount = 4;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format(RegMemFormat, reg.W));

            // mod reg r/m
            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("{0}{1}{2}", "00", reg, "110"));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForMemReg(TranslationContext context, int startPos)
        {
            Identifier memCell = null;
            Register reg = null;
            context.Checker.CheckMemReg(Tokens, startPos, out memCell, out reg);

            int bytesCount = 4;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format(MemRegFormat, reg.W));

            // mod reg r/m
            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("{0}{1}{2}", "00", reg, "110"));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForRegIm(TranslationContext context, int startPos)
        {
            Register reg = null;
            Constant constant = null;
            context.Checker.CheckRegIm(Tokens, startPos, out reg, out constant);

            context.AddTranslatedUnit(GetTranslatedBytesForRegIm(reg, constant));
        }
        private void translateForMemIm(TranslationContext context, int startPos)
        {
            Identifier identifier = null;
            Constant constant = null;
            context.Checker.CheckMemIm(Tokens, startPos, out identifier, out constant);

            context.AddTranslatedUnit(GetTranslatedBytesForMemIm(identifier, constant));
        }

        protected override void TranslateCommand(TranslationContext context, int startPos)
        {
            switch (OperandsSetType)
            {
                case OperandsSetType.RR: translateForRegReg(context, startPos); break;
                case OperandsSetType.RM: translateForRegMem(context, startPos); break;
                case OperandsSetType.MR: translateForMemReg(context, startPos); break;
                case OperandsSetType.RI: translateForRegIm(context,  startPos); break;
                case OperandsSetType.MI: translateForMemIm(context,  startPos); break;

                default: 
                    ThrowForUnsupportedOST(OperandsSetType, Tokens[startPos].Position); break;
            }
        }
    }
}
