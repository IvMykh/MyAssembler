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

        protected override void EndTranslation(TranslationContext context, Constant constant)
        {
            context.AddTranslatedUnit(constant.Bytes);
        }
    }
}
