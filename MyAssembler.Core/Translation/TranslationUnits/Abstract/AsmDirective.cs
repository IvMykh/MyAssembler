using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Properties;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.OperandsTypeChecking;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class AsmDirective
        : AsmTranslationUnit
    {
        protected abstract int CellSize { get; }

        public AsmDirective(List<Token> tokens)
            : base(tokens)
        {
        }

        protected abstract void InsertTranslatedBytes(TranslationContext context, byte[] translatedBytes);
        protected abstract void CollectMemoryCellAddress(TranslationContext context, string identifier);


        protected override sealed void Translate(TranslationContext context)
        {
            Token constToken = Tokens[2];
            byte[] translatedBytes = null;

            if (constToken.Type != TokenType.QuestionMark)
            {
                var constant = new Constant(constToken.Value, constToken.Type);

                if (constant.Bytes.Length > CellSize)
                {
                    throw new TranslationErrorException(
                        string.Format(Resources.InitializerOverflowMsgFormat,
                            constant.Value,
                            CellSize,
                            (CellSize > 1) ? "s" : ""));
                }

                translatedBytes = constant.Bytes;
            }
            else
            {
                translatedBytes = new byte[CellSize];
            }

            InsertTranslatedBytes(context, translatedBytes);
            CollectMemoryCellAddress(context, Tokens[0].Value);
        }

        protected override void PerformAddressInsertion(TranslationContext context) 
        { /* Do nothing. */ }
    }
}
