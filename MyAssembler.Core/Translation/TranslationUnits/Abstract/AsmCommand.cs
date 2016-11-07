using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Properties;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class AsmCommand
        : AsmTranslationUnit
    {
        public OperandsSetType OperandsSetType { get; protected set; }

        public AsmCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens)
        {
            OperandsSetType = operandsSetType;
        }

        protected abstract void TranslateCommand(TranslationContext context, int startPos);

        protected override sealed void Translate(TranslationContext context)
        {
            string label = null;
            int i = 1;

            if (Tokens[0].Type == TokenType.Identifier)
            {
                label = Tokens[0].Value;

                // Skip label and ':' tokens.
                ++i;
                ++i;
            }

            TranslateCommand(context, i);

            if (label != null)
            {
                short address = context.StartAddresses[context.UnitCursor];
                context.MemoryManager.InsertLabelAddress(label, address);
            }

        }

        protected abstract void UseAddress(TranslationContext context, short address);

        protected override void PerformAddressInsertion(TranslationContext context)
        {
            // If we deal with memory cell...
            if (OperandsSetType == OperandsSetType.M  ||
                OperandsSetType == OperandsSetType.AM ||
                OperandsSetType == OperandsSetType.MI ||
                OperandsSetType == OperandsSetType.MR ||
                OperandsSetType == OperandsSetType.RM ||
                OperandsSetType == OperandsSetType.RMI)
            {
                Token idToken = Tokens.FindLast(t => t.Type == TokenType.Identifier);
                IdentifierType idType = context.MemoryManager.GetIdentifierType(idToken.Value);

                short address = context.MemoryManager.GetAddressFor(idToken.Value);

                UseAddress(context, address);
            }
        }

        protected void ThrowForUnsupportedOST(OperandsSetType ost, TokenPosition pos)
        {
            throw new TranslationErrorException(
                string.Format(Resources.OSTNotSupportedMsgFormat,
                    ost,
                    pos.Line,
                    pos.StartIndex));
        }
    }
}
