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

        protected abstract void EndTranslation(TranslationContext context, Constant constant);

        protected override sealed void Translate(TranslationContext context)
        {
            Token constToken = Tokens[2];

            if (constToken.Type == TokenType.QuestionMark)
            {
                byte[] translatedBytes = new byte[CellSize];
                context.AddTranslatedUnit(translatedBytes);
                return;
            }

            var constant = new Constant(constToken.Value, constToken.Type);

            if (constant.Bytes.Length > CellSize)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.InitializerOverflowMsgFormat,
                        constant.Value,
                        CellSize,
                        (CellSize > 1) ? "s" : ""));
            }

            EndTranslation(context, constant);
        }
    }
}
