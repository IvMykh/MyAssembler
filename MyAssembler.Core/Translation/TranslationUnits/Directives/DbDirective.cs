using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Properties;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.OperandsTypeChecking;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Directives
{
    public class DbDirective
        : AsmDirective
    {
        protected override int CellSize
        {
            get { return 1; }
        }

        public DbDirective(List<Token> tokens)
            : base(tokens)
        {
        }

        protected override void InsertTranslatedBytes(TranslationContext context, byte[] translatedBytes)
        {
            context.AddTranslatedUnit(translatedBytes);
        }

        protected override void CollectMemoryCellAddress(TranslationContext context, string identifier)
        {
            short address = context.StartAddresses[context.UnitCursor];
            context.MemoryManager.InsertByteCellAddress(identifier, address);
        }

        protected override void CheckForOverflow(TokenType constTokenType, Constant constant)
        {
            if (constTokenType != TokenType.Literal && constant.Bytes.Length > CellSize)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.InitializerOverflowMsgFormat,
                        constant.Value,
                        CellSize,
                        (CellSize > 1) ? "s" : ""));
            }
        }
    }
}
