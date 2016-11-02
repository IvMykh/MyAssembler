using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants;

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

        protected virtual byte[] GetTranslatedBytesForRegIm(
            TranslationContext context, RegisterType register, byte[] constBytes)
        {
            WValue w1 = context.WValueHelper.WValueForRegister(register);

            int bytesCount = (w1 == WValue.Zero) ? 3 : 2 + constBytes.Length;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                    string.Format(RegImFormat,
                        (w1 == WValue.One && constBytes.Length == 1) ? 1 : 0,
                        context.WValueHelper.WValueToString(w1)));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                    string.Format("11{0}{1}",
                        RegFieldForIm,
                        context.RegisterHelper.RegisterToBitString(register)));

            translatedBytes[2] = constBytes[0];

            if (translatedBytes.Length == 4)
            {
                translatedBytes[3] = constBytes[1];
            }

            return translatedBytes;
        }
        
        protected virtual byte[] GetTranslatedBytesForMemIm(
            TranslationContext context, string identifier, byte[] constBytes)
        {
            IdentifierType idType = context.MemoryManager.GetIdentifierType(identifier);

            int bytesCount = (idType == IdentifierType.Byte) ? 5 : 4 + constBytes.Length;
            byte[] translatedBytes = new byte[bytesCount];

            int w = (idType == IdentifierType.Byte) ? 0 : 1;

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                    string.Format(MemImFormat,
                        (idType == IdentifierType.Word && constBytes.Length == 1) ? 1 : 0,
                        w.ToString()));

            // mod reg r/m
            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("00{0}110",
                    RegFieldForIm));

            // Address.
            translatedBytes[2] = 0x00;
            translatedBytes[3] = 0x00;

            translatedBytes[4] = constBytes[0];

            if (translatedBytes.Length == 6)
            {
                translatedBytes[5] = constBytes[1];
            }

            return translatedBytes;
        }


        private void translateForRegReg(TranslationContext context, int startPos)
        {
            RegisterType firstReg = context.RegisterHelper.Parse(Tokens[startPos++].Value);
            ++startPos; // Skipping comma.
            RegisterType secondReg = context.RegisterHelper.Parse(Tokens[startPos++].Value);

            CheckForRegRegMismatch(context, firstReg, secondReg);

            WValue w1 = context.WValueHelper.WValueForRegister(firstReg);
            
            var translatedBytes = new byte[2];
            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format(RegRegFormat,
                    context.WValueHelper.WValueToString(w1)));

            // mod reg r/m
            translatedBytes[1] = BitStringHelper.BitStringToByte(
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
            byte[] translatedBytes = new byte[4];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format(RegMemFormat,
                    context.WValueHelper.WValueToString(w1)));

            // mod reg r/m
            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("{0}{1}{2}",
                    "00",
                    context.RegisterHelper.RegisterToBitString(register),
                    "110"));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForMemReg(TranslationContext context, int startPos)
        {
            string identifier = Tokens[startPos++].Value;
            ++startPos; // Skip comma.
            RegisterType register = context.RegisterHelper.Parse(Tokens[startPos++].Value);

            CheckForRegMemMismatch(context, register, identifier);

            WValue w2 = context.WValueHelper.WValueForRegister(register);
            byte[] translatedBytes = new byte[4];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format(MemRegFormat,
                    context.WValueHelper.WValueToString(w2)));

            // mod reg r/m
            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("{0}{1}{2}",
                    "00",
                    context.RegisterHelper.RegisterToBitString(register),
                    "110"));

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForRegIm(TranslationContext context, int startPos)
        {
            RegisterType register = context.RegisterHelper.Parse(Tokens[startPos++].Value);
            ++startPos; // Skipping comma.
            Token constToken = Tokens[startPos++];

            CheckForRegImMismatch(context, register, constToken);

            ConstantsParser parser = GetConstsParser(context, constToken.Type);
            byte[] constBytes = parser.Parse(constToken.Value);
            
            context.AddTranslatedUnit(GetTranslatedBytesForRegIm(context, register, constBytes));
        }
        private void translateForMemIm(TranslationContext context, int startPos)
        {
            string identifier = Tokens[startPos++].Value;
            ++startPos; // Skipping comma.
            Token constToken = Tokens[startPos++];

            CheckForMemImMismatch(context, identifier, constToken);

            ConstantsParser parser = GetConstsParser(context, constToken.Type);
            byte[] constBytes = parser.Parse(constToken.Value);

            context.AddTranslatedUnit(GetTranslatedBytesForMemIm(context, identifier, constBytes));
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
                case OperandsSetType.RR: translateForRegReg(context, i); break;
                case OperandsSetType.RM: translateForRegMem(context, i); break;
                case OperandsSetType.MR: translateForMemReg(context, i); break;
                case OperandsSetType.RI: translateForRegIm(context, i); break;
                case OperandsSetType.MI: translateForMemIm(context, i); break;

                default: throw new TranslationErrorException(
                    string.Format("Add: operands set {0} is not supported.",
                        OperandsSetType));
            }
        }
    }
}
