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
                short address = context.StartAddresses[context.StartAddresses.Count - 1];
                context.MemoryManager.InsertLabelAddress(label, address);
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
