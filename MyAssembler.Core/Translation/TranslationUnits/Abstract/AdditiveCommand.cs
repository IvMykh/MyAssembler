using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class AdditiveCommand
        : AsmCommand
    {
        public AdditiveCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        protected abstract string RegRegFormat { get; }
        protected abstract string RegMemFormat { get; }
        protected abstract string MemRegFormat { get; }
        protected abstract string RegImFormat  { get; }
        protected abstract string MemImFormat  { get; }

        protected abstract string RegFieldForIm { get; }

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

            WValue w1 = context.WValueHelper.WValueForRegister(register);

            byte[] translatedBytes = null;
            int bytesCount = 0;
            int s = 0;

            if (w1 == WValue.Zero)
            {
                bytesCount = 3;
                translatedBytes = new byte[bytesCount];
            }
            else
            {
                s = (constBytes.Length == 1) ? 1 : 0;
                bytesCount = 2 + constBytes.Length;
            }

            translatedBytes = new byte[bytesCount];

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

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForMemIm(TranslationContext context, int startPos)
        {
            string identifier = Tokens[startPos++].Value;
            ++startPos; // Skipping comma.
            Token constToken = Tokens[startPos++];

            CheckForMemImMismatch(context, identifier, constToken);

            ConstantsParser parser = GetConstsParser(context, constToken.Type);
            byte[] constBytes = parser.Parse(constToken.Value);

            IdentifierType idType = context.MemoryManager.GetIdentifierType(identifier);

            byte[] translatedBytes = null;
            int bytesCount = 0;
            int s = 0;

            if (idType == IdentifierType.Byte)
            {
                bytesCount = 5;
                translatedBytes = new byte[bytesCount];
            }
            else
            {
                s = (constBytes.Length == 1) ? 1 : 0;
                bytesCount = 4 + constBytes.Length;
            }

            translatedBytes = new byte[bytesCount];
            int wForIdentifier = (idType == IdentifierType.Byte) ? 0 : 1;

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                    string.Format(MemImFormat,
                        (idType == IdentifierType.Word && constBytes.Length == 1) ? 1 : 0,
                        wForIdentifier.ToString()));

            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("00{0}110",
                    RegFieldForIm));

            // address - skip.
            // translatedBytes[2]
            // translatedBytes[3]

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
