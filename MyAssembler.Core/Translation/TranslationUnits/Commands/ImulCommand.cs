using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.OperandsTypeChecking;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    public class ImulCommand
        : AsmCommand
    {
        public ImulCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        private void translateForAReg(TranslationContext context, int startPos)
        {
            Register reg = null;
            context.Checker.CheckReg(Tokens, startPos, out reg);

            int bytesCount = 2;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1111011{0}", reg.W));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("11101{0}", reg));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForAMem(TranslationContext context, int startPos)
        {
            Identifier identifier = null;
            context.Checker.CheckMem(Tokens, startPos, out identifier);

            int bytesCount = 4;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1111011{0}", identifier.W));

            translatedBytes[1] = BitStringHelper.BitStringToByte("00101110");

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForRegReg(TranslationContext context, int startPos)
        {
            Register reg1 = null;
            Register reg2 = null;
            context.Checker.CheckRegReg(Tokens, startPos, out reg1, out reg2);

            int bytesCount = 3;
            var translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte("00001111");
            translatedBytes[1] = BitStringHelper.BitStringToByte("10101111");

            // mod reg r/m
            translatedBytes[2] = BitStringHelper.BitStringToByte(
                string.Format("{0}{1}{2}", "11", reg1, reg2));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForRegMem(TranslationContext context, int startPos)
        {
            Register reg = null;
            Identifier identifier = null;
            context.Checker.CheckRegMem(Tokens, startPos, out reg, out identifier);

            int bytesCount = 5;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte("00001111");
            translatedBytes[1] = BitStringHelper.BitStringToByte("10101111");

            // mod reg r/m
            translatedBytes[2] = BitStringHelper.BitStringToByte(
                string.Format("{0}{1}{2}", "00", reg, "110"));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForRegRegIm(TranslationContext context, int startPos)
        {
            Register reg1 = null;
            Register reg2 = null;
            Constant constant = null;
            context.Checker.CheckRegRegIm(Tokens, startPos, out reg1, out reg2, out constant);

            int bytesCount = (reg1.W == WValueStore.ZERO) ? 3 : 2 + constant.Bytes.Length;
            byte[] translatedBytes = new byte[bytesCount];

            int s = (reg1.W == WValueStore.ONE && constant.Bytes.Length == 1) ? 
                1 : 0;

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("011010{0}1", s));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                    string.Format("11{0}{1}", reg1, reg2));

            translatedBytes[2] = constant.Bytes[0];

            if (translatedBytes.Length == 4)
            {
                translatedBytes[3] = constant.Bytes[1];
            }

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForRegMemIm(TranslationContext context, int startPos)
        {
            Register reg = null;
            Identifier id = null;
            Constant constant = null;
            context.Checker.CheckRegMemIm(Tokens, startPos, out reg, out id, out constant);

            int bytesCount = (reg.W == WValueStore.ZERO) ? 5 : 4 + constant.Bytes.Length;
            byte[] translatedBytes = new byte[bytesCount];

            int s = (reg.W == WValueStore.ONE && constant.Bytes.Length == 1) ? 
                1 : 0;

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("011010{0}1", s));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("00{0}110", reg));

            // Address.
            translatedBytes[2] = 0x00;
            translatedBytes[3] = 0x00;

            translatedBytes[4] = constant.Bytes[0];

            if (translatedBytes.Length == 6)
            {
                translatedBytes[5] = constant.Bytes[1];
            }

            context.AddTranslatedUnit(translatedBytes);
        }

        protected override void TranslateCommand(TranslationContext context, int startPos)
        {
            switch (OperandsSetType)
            {
                case OperandsSetType.AR: translateForAReg(context, startPos); break;
                case OperandsSetType.AM: translateForAMem(context, startPos); break;
                case OperandsSetType.RR: translateForRegReg(context, startPos); break;
                case OperandsSetType.RM: translateForRegMem(context, startPos); break;
                case OperandsSetType.RRI: translateForRegRegIm(context, startPos); break;
                case OperandsSetType.RMI: translateForRegMemIm(context, startPos); break;

                default: 
                    ThrowForUnsupportedOST(OperandsSetType, Tokens[startPos].Position); break;
            }
        }
    }
}
