using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants;
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
            RegisterType register = context.RegisterHelper.Parse(Tokens[startPos].Value);

            int bytesCount = 2;
            byte[] translatedBytes = new byte[bytesCount];

            WValue w = context.WValueHelper.WValueForRegister(register);

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1111011{0}",
                    context.WValueHelper.WValueToString(w)));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("11101{0}",
                    context.RegisterHelper.RegisterToBitString(register)));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForAMem(TranslationContext context, int startPos)
        {
            string identifier = Tokens[startPos++].Value;
            IdentifierType idType = context.MemoryManager.GetIdentifierType(identifier);

            if (idType == IdentifierType.Label)
            {
                throw new TranslationErrorException(
                    string.Format("{0}: label identifier is not valid in this context.",
                        identifier));
            }

            int w = (idType == IdentifierType.Byte) ? 0 : 1;

            int bytesCount = 4;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1111011{0}",
                    w.ToString()));

            translatedBytes[1] = BitStringHelper.BitStringToByte("00101110");

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForRegReg(TranslationContext context, int startPos)
        {
            RegisterType firstReg = context.RegisterHelper.Parse(Tokens[startPos++].Value);
            ++startPos; // Skipping comma.
            RegisterType secondReg = context.RegisterHelper.Parse(Tokens[startPos++].Value);

            WValue w1 = context.WValueHelper.WValueForRegister(firstReg);
            WValue w2 = context.WValueHelper.WValueForRegister(secondReg);

            if (w1 != w2)
            {
                throw new TranslationErrorException(
                    string.Format("{0} and {1}: operands type mismatch.",
                        firstReg,
                        secondReg));
            }

            int bytesCount = 3;
            var translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte("00001111");
            translatedBytes[1] = BitStringHelper.BitStringToByte("10101111");

            // mod reg r/m
            translatedBytes[2] = BitStringHelper.BitStringToByte(
                string.Format("{0}{1}{2}",
                    "11",
                    context.RegisterHelper.RegisterToBitString(firstReg),
                    context.RegisterHelper.RegisterToBitString(secondReg)));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForRegMem(TranslationContext context, int startPos)
        {
            RegisterType register = context.RegisterHelper.Parse(Tokens[startPos++].Value);
            ++startPos; // Skip comma.
            string identifier = Tokens[startPos++].Value;

            CheckForRegMemMismatch(context, register, identifier);

            WValue w1 = context.WValueHelper.WValueForRegister(register);

            int bytesCount = 5;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte("00001111");
            translatedBytes[1] = BitStringHelper.BitStringToByte("10101111");

            // mod reg r/m
            translatedBytes[2] = BitStringHelper.BitStringToByte(
                string.Format("{0}{1}{2}",
                    "00",
                    context.RegisterHelper.RegisterToBitString(register),
                    "110"));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForRegRegIm(TranslationContext context, int startPos)
        {
            RegisterType firstReg = context.RegisterHelper.Parse(Tokens[startPos++].Value);
            ++startPos; // Skipping comma.
            RegisterType secondReg = context.RegisterHelper.Parse(Tokens[startPos++].Value);
            ++startPos; // Skipping comma.
            Token constToken = Tokens[startPos++];

            CheckForRegRegMismatch(context, firstReg, secondReg);
            CheckForRegImMismatch(context, firstReg, constToken);

            WValue w1 = context.WValueHelper.WValueForRegister(firstReg);
            
            ConstantsParser parser = GetConstsParser(context, constToken.Type);
            byte[] constBytes = parser.Parse(constToken.Value);
            
            int bytesCount = (w1 == WValue.Zero) ? 3 : 2 + constBytes.Length;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("011010{0}1",
                    (w1 == WValue.One && constBytes.Length == 1) ? 1 : 0));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                    string.Format("11{0}{1}",
                        context.RegisterHelper.RegisterToBitString(firstReg),
                        context.RegisterHelper.RegisterToBitString(secondReg)));

            translatedBytes[2] = constBytes[0];

            if (translatedBytes.Length == 4)
            {
                translatedBytes[3] = constBytes[1];
            }

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForRegMemIm(TranslationContext context, int startPos)
        {
            RegisterType register = context.RegisterHelper.Parse(Tokens[startPos++].Value);
            ++startPos; // Skipping comma.
            string identifier = Tokens[startPos++].Value;
            ++startPos; // Skipping comma.
            Token constToken = Tokens[startPos++];

            CheckForRegMemMismatch(context, register, identifier);
            CheckForRegImMismatch(context, register, constToken);

            WValue w = context.WValueHelper.WValueForRegister(register);

            ConstantsParser parser = GetConstsParser(context, constToken.Type);
            byte[] constBytes = parser.Parse(constToken.Value);

            int bytesCount = (w == WValue.Zero) ? 5 : 4 + constBytes.Length;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("011010{0}1",
                    (w == WValue.One && constBytes.Length == 1) ? 1 : 0));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("00{0}110",
                    context.RegisterHelper.RegisterToBitString(register)));

            // Address.
            translatedBytes[2] = 0x00;
            translatedBytes[3] = 0x00;

            translatedBytes[4] = constBytes[0];

            if (translatedBytes.Length == 6)
            {
                translatedBytes[5] = constBytes[1];
            }

            context.AddTranslatedUnit(translatedBytes);
        }
        

        protected override void Translate(TranslationContext context)
        {
            int i = 1;
            if (Tokens[0].Type == TokenType.Identifier)
            {
                // Skip label and ':' tokens.
                ++i;
                ++i;
            }

            switch (OperandsSetType)
            {
                case OperandsSetType.AR: translateForAReg(context, i); break;
                case OperandsSetType.AM: translateForAMem(context, i); break;
                case OperandsSetType.RR: translateForRegReg(context, i); break;
                case OperandsSetType.RM: translateForRegMem(context, i); break;
                case OperandsSetType.RRI: translateForRegRegIm(context, i); break;
                case OperandsSetType.RMI: translateForRegMemIm(context, i); break;

                default: throw new TranslationErrorException(
                    string.Format("Add: operands set {0} is not supported.",
                        OperandsSetType));
            }
        }
    }
}
