using System;
using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ParsersForConstants;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    using BitStrHelper = BitStringManipHelper;

    /*
     * R - R - 1000 100w mod reg r/m
     * R - M - 1000 101w mod reg r/m
     * M - R - 1000 100w mod reg r/m
     * R - I - 1011w reg im8, 16, 32
     * M - I - 1100 011w mod 000 r/m
     */
    public class MovCommand
        : AsmCommand
    {
        public MovCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        // MOV BX,AX
        private void translateForRR(TranslationContext context, int startPos)
        {
            RegisterType firstReg = (RegisterType)Enum.Parse(
                typeof(RegisterType), Tokens[startPos++].Value);
            RegisterBitsMapResult mapRes1 = context.RegisterBitsMapper.Map(firstReg);

            ++startPos; // Skipping comma.

            RegisterType secondReg = (RegisterType)Enum.Parse(
                typeof(RegisterType), Tokens[startPos++].Value);
            RegisterBitsMapResult mapRes2 = context.RegisterBitsMapper.Map(secondReg);

            if (mapRes1.W != mapRes2.W)
            {
                throw new TranslationErrorException(
                    string.Format("{0} and {1}: operands type mismatch.",
                        firstReg, 
                        secondReg));
            }

            var translatedBytes = new byte[2];

            translatedBytes[0] = BitStrHelper.BitStringToByte(
                string.Format("1000101{0}", mapRes1.W));

            // mod reg r/m
            translatedBytes[1] = BitStrHelper.BitStringToByte(
                string.Format("{0}{1}{2}",
                    "11",
                    mapRes1.BitsString,
                    mapRes2.BitsString));

            context.AddTranslatedUnit(translatedBytes);
        }

        // MOV BX,memcell
        private void translateForRM(TranslationContext context, int startPos)
        {
            RegisterType register = (RegisterType)Enum.Parse(
                typeof(RegisterType), Tokens[startPos++].Value);
            RegisterBitsMapResult mapRes = context.RegisterBitsMapper.Map(register);

            byte[] translatedBytes = null;

            translatedBytes = new byte[4];

            translatedBytes[0] = BitStrHelper.BitStringToByte(
                string.Format("1000101{0}", mapRes.W));

            // mod reg r/m
            translatedBytes[1] = BitStrHelper.BitStringToByte(
                string.Format("{0}{1}{2}",
                    "00",
                    mapRes.BitsString,
                    "110"));

            context.AddTranslatedUnit(translatedBytes);
        }
        
        // MOV memcell,BX
        private void translateForMR(TranslationContext context, int startPos)
        {
            ++startPos; // Skip identifier.
            ++startPos; // Skip comma.

            RegisterType register = (RegisterType)Enum.Parse(
                typeof(RegisterType), Tokens[startPos++].Value);
            RegisterBitsMapResult mapRes = context.RegisterBitsMapper.Map(register);

            byte[] translatedBytes = new byte[4];

            translatedBytes[0] = BitStrHelper.BitStringToByte(
                string.Format("1000100{0}", mapRes.W));

            // mod reg r/m
            translatedBytes[1] = BitStrHelper.BitStringToByte(
                string.Format("{0}{1}{2}",
                    "00",
                    mapRes.BitsString,
                    "110"));

            context.AddTranslatedUnit(translatedBytes);
        }
        
        // MOV BX,12
        private void translateForRI(TranslationContext context, int startPos) 
        {
            RegisterType register = (RegisterType)Enum.Parse(
                typeof(RegisterType), Tokens[startPos++].Value);
            RegisterBitsMapResult mapRes = context.RegisterBitsMapper.Map(register);

            ++startPos; // Skipping comma.

            var constToken = Tokens[startPos++];
            
            ConstantsParser parser = getParser(context, constToken.Type);
            byte[] constBytes = parser.Parse(constToken.Value);

            if (context.RegisterBitsMapper.WStringToByte(mapRes.W) == 0 && 
                constBytes.Length == 2)
            {
                throw new TranslationErrorException(
                    string.Format("{0} and {1}: operands type mismatch.",
                        register,
                        constToken.Value));
            }

            // 2 or 3
            int bytesCount = 2 + (context.RegisterBitsMapper.WStringToByte(mapRes.W));
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStrHelper.BitStringToByte(
                string.Format("1011{0}{1}", 
                    mapRes.W, 
                    mapRes.BitsString));

            translatedBytes[1] = constBytes[0];
            
            if (constBytes.Length == 2)
            {
                translatedBytes[2] = constBytes[1];
            }

            context.AddTranslatedUnit(translatedBytes);
        }

        // MOV memcell,12
        private void translateForMI(TranslationContext context, int startPos) 
        {
            // TODO: collect info about memory cells before translating this command.
            // (either it is byte or word).



            throw new NotImplementedException();
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


        public override void Translate(TranslationContext context)
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
                case OperandsSetType.RR: translateForRR(context, i); break;
                case OperandsSetType.RM: translateForRM(context, i); break;
                case OperandsSetType.MR: translateForMR(context, i); break;
                case OperandsSetType.RI: translateForRI(context, i); break;
                case OperandsSetType.MI: translateForMI(context, i); break;

                default: throw new TranslationErrorException(
                    string.Format("MOV: operands set {0} is not supported.", 
                        OperandsSetType));
            }
        }
    }
}
