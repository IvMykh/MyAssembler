using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.OperandsTypeChecking;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Directives
{
    public class DwDirective
        : AsmDirective
    {
        protected override int CellSize
        {
            get { return 2; }
        }

        public DwDirective(List<Token> tokens)
            : base(tokens)
        {
        }

        protected override void EndTranslation(TranslationContext context, Constant constant)
        {
            if (constant.Bytes.Length == 1)
            {
                context.AddTranslatedUnit(new byte[2] { 0x00, constant.Bytes[0] });
            }
            else
            {
                context.AddTranslatedUnit(constant.Bytes);
            }
        }
    }
}
