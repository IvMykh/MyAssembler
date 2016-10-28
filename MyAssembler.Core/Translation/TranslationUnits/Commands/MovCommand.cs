using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ParsersForConstants;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    public class MovCommand
        : AsmCommand
    {
        public MovCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        private void checkForRegMemMismatch(
            TranslationContext context, RegisterType register, string identifier)
        {
            WValue wForReg = context.WValueHelper.WValueForRegister(register);
            IdentifierType idType = context.MemoryManager.GetIdentifierType(identifier);
            
            if (idType == IdentifierType.Label)
            {
                throw new TranslationErrorException(
                    string.Format("{0}: label identifier is not valid in this context.",
                        identifier));
            }

            if (idType == IdentifierType.Byte && wForReg == WValue.One ||
                idType == IdentifierType.Word && wForReg == WValue.Zero)
            {
                throw new TranslationErrorException(
                    string.Format("{0} and {1}: operands type mismatch.",
                        register,
                        identifier));
            }
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

            var translatedBytes = new byte[2];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1000101{0}", 
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

            checkForRegMemMismatch(context, register, identifier);

            WValue w1 = context.WValueHelper.WValueForRegister(register);
            byte[] translatedBytes = new byte[4];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1000101{0}", 
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

            checkForRegMemMismatch(context, register, identifier);

            WValue w2 = context.WValueHelper.WValueForRegister(register);
            byte[] translatedBytes = new byte[4];

            translatedBytes[0] = BitStringHelper.BitStringToByte(
                string.Format("1000100{0}", 
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
            
            ConstantsParser parser = getParser(context, constToken.Type);
            byte[] constBytes = parser.Parse(constToken.Value);

            WValue w1 = context.WValueHelper.WValueForRegister(register);

            if (w1 == WValue.Zero && constBytes.Length == 2)
            {
                throw new TranslationErrorException(
                    string.Format("{0} and {1}: operands type mismatch.",
                        register,
                        constToken.Value));
            }

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

            context.AddTranslatedUnit(translatedBytes);
        }
        private void translateForMemIm(TranslationContext context, int startPos) 
        {
            string identifier = Tokens[startPos++].Value;
            ++startPos; // Skipping comma.
            Token constToken = Tokens[startPos++];

            ConstantsParser parser = getParser(context, constToken.Type);
            byte[] constBytes = parser.Parse(constToken.Value);

            IdentifierType idType = context.MemoryManager.GetIdentifierType(identifier);

            if (idType == IdentifierType.Label)
            {
                throw new TranslationErrorException(
                    string.Format("{0}: label identifier is not valid in this context.",
                        identifier));
            }

            if (idType == IdentifierType.Byte && constBytes.Length == 2)
            {
                throw new TranslationErrorException(
                    string.Format("{0} and {1}: operands type mismatch.",
                        identifier,
                        constToken.Value));
            }

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

            context.AddTranslatedUnit(translatedBytes);
        }

        private ConstantsParser getParser(TranslationContext context, TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.BinConstant: return context.BinConstsParser;
                case TokenType.DecConstant: return context.DecConstsParser;
                case TokenType.HexConstant: return context.HexConstsParser;

                default: throw new TranslationErrorException(
                    string.Format("{0}: such constant type is not supported.", 
                        tokenType));
            }
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
                    string.Format("MOV: operands set {0} is not supported.", 
                        OperandsSetType));
            }
        }
    }
}
