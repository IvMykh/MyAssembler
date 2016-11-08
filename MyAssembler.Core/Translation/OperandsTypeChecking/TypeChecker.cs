using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Properties;
using MyAssembler.Core.Translation.ContextInfrastructure;

namespace MyAssembler.Core.Translation.OperandsTypeChecking
{
    public class TypeChecker
    {
        private TranslationContext _context;

        public TypeChecker(TranslationContext context)
        {
            _context = context;
        }

        private void excludeUnexpectedLabel(Token idToken, IdentifierType type)
        {
            if (type == IdentifierType.Label)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.UnexpectedLabelMsgFormat,
                        idToken.Value,
                        idToken.Position.Line,
                        idToken.Position.StartIndex));
            }
        }
        private void excludeRegRegMismatch(Register reg1, Register reg2, TokenPosition pos)
        {
            if (reg1.W != reg2.W)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.RegRegMismatchMsgFormat,
                        reg1.Type,
                        reg2.Type,
                        pos.Line,
                        pos.StartIndex));
            }
        }
        private void excludeRegImMismatch(Register reg, Constant constant, TokenPosition pos)
        {
            if (reg.W == WValueStore.ZERO && constant.W == WValueStore.ONE)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.RegImMismatchMsgFormat,
                        reg.Type,
                        constant.Value,
                        pos.Line,
                        pos.StartIndex));
            }
        }
        private void excludeRegMemMismatch(Register reg, Identifier id, TokenPosition pos)
        {
            if (reg.W != id.W)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.RegMemMismatchMsgFormat,
                        reg.Type,
                        id.Name,
                        id.Type,
                        pos.Line,
                        pos.StartIndex));
            }
        }
        private void excludeMemImMismatch(Identifier id, Constant constant, TokenPosition pos)
        {
            if (id.W == WValueStore.ZERO && constant.W == WValueStore.ONE)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.MemImMismatchMsgFormat,
                        id.Name,
                        id.Type,
                        constant.Value,
                        pos.Line,
                        pos.StartIndex));
            }
        }

        public void CheckLabel(List<Token> tokens, int startPos, out Identifier id)
        {
            Token idToken = tokens[startPos];

            id = new Identifier(idToken.Value, _context.MemoryManager.GetIdentifierType(idToken.Value));
            
            if (id.Type != IdentifierType.Label)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.ExpectedLabelMsgFormat,
                        idToken.Value,
                        idToken.Position.Line,
                        idToken.Position.StartIndex));
            }
        }
        public void CheckReg(List<Token> tokens, int startPos, out Register reg)
        {
            reg = new Register(tokens[startPos].Value);
        }
        public void CheckMem(List<Token> tokens, int startPos, out Identifier id)
        { 
            Token idToken = tokens[startPos];
            id = new Identifier(idToken.Value, _context.MemoryManager.GetIdentifierType(idToken.Value));

            excludeUnexpectedLabel(idToken, id.Type);
        }
        public void CheckIm8(List<Token> tokens, int startPos, out Constant constant)
        {
            var constToken = tokens[startPos];
            constant = new Constant(constToken.Value, constToken.Type);

            if (constant.Bytes.Length > 1)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.ConstantOverflowMsgFormat, constant.Value));
            }
        }

        public void CheckRegReg(
            List<Token> tokens, int startPos, out Register reg1, out Register reg2)
        {
            Token reg1Token = tokens[startPos++];
            ++startPos;
            Token reg2Token = tokens[startPos++];

            reg1 = new Register(reg1Token.Value);
            reg2 = new Register(reg2Token.Value);

            excludeRegRegMismatch(reg1, reg2, reg1Token.Position);
        }

        void checkForRegisterAndIdentifier(
            Token regToken, Token idToken, out Register reg, out Identifier id)
        {
            reg = new Register(regToken.Value);
            id = new Identifier(idToken.Value, _context.MemoryManager.GetIdentifierType(idToken.Value));

            excludeUnexpectedLabel(idToken, id.Type);
            excludeRegMemMismatch(reg, id, regToken.Position);
        }

        public void CheckRegMem(
            List<Token> tokens, int startPos, out Register reg, out Identifier id)
        {
            Token regToken = tokens[startPos++];
            ++startPos;
            Token idToken = tokens[startPos++];

            checkForRegisterAndIdentifier(regToken, idToken, out reg, out id);
        }

        public void CheckMemReg(
            List<Token> tokens, int startPos, out Identifier id, out Register reg)
        {
            Token idToken = tokens[startPos++];
            ++startPos;
            Token regToken = tokens[startPos++];

            checkForRegisterAndIdentifier(regToken, idToken, out reg, out id);
        }

        public void CheckRegIm(
            List<Token> tokens, int startPos, out Register reg, out Constant constant)
        {
            Token regToken = tokens[startPos++];
            ++startPos; // Skipping comma.
            Token constToken = tokens[startPos++];

            reg = new Register(regToken.Value);
            constant = new Constant(constToken.Value, constToken.Type);

            excludeRegImMismatch(reg, constant, regToken.Position);
        }

        public void CheckMemIm(
            List<Token> tokens, int startPos, out Identifier id, out Constant constant)
        {
            Token idToken = tokens[startPos++];
            ++startPos; // Skipping comma.
            Token constToken = tokens[startPos++];

            id = new Identifier(idToken.Value, _context.MemoryManager.GetIdentifierType(idToken.Value));
            constant = new Constant(constToken.Value, constToken.Type);

            excludeUnexpectedLabel(idToken, id.Type);
            excludeMemImMismatch(id, constant, idToken.Position);
        }


        public void CheckRegRegIm(
            List<Token> tokens, int startPos, out Register reg1, out Register reg2, out Constant constant)
        {
            Token reg1Token = tokens[startPos++];
            ++startPos;
            Token reg2Token = tokens[startPos++];
            ++startPos;
            Token constToken = tokens[startPos++];

            reg1 = new Register(reg1Token.Value);
            reg2 = new Register(reg2Token.Value);
            constant = new Constant(constToken.Value, constToken.Type);

            excludeRegRegMismatch(reg1, reg2, reg1Token.Position);
            excludeRegImMismatch(reg1, constant, reg1Token.Position);
        }
        public void CheckRegMemIm(
           List<Token> tokens, int startPos, out Register reg, out Identifier id, out Constant constant)
        {
            Token regToken = tokens[startPos++];
            ++startPos;
            Token idToken = tokens[startPos++];
            ++startPos;
            Token constToken = tokens[startPos++];

            reg = new Register(regToken.Value);
            id = new Identifier(idToken.Value, _context.MemoryManager.GetIdentifierType(idToken.Value));
            constant = new Constant(constToken.Value, constToken.Type);

            excludeRegMemMismatch(reg, id, regToken.Position);
            excludeRegImMismatch(reg, constant, regToken.Position);
        }

    }
}
