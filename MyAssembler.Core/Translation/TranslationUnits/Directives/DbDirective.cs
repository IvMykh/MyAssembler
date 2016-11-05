using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
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

        protected override void InsertTranslatedBytes(TranslationContext context, Constant constant)
        {
            context.AddTranslatedUnit(constant.Bytes);
        }

        protected override void CollectMemoryCellAddress(TranslationContext context, string identifier)
        {
            short address = context.StartAddresses[context.StartAddresses.Count - 1];
            context.MemoryManager.InsertByteCellAddress(identifier, address);
        }
    }
}
