using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class AsmDirective
        : AsmTranslationUnit
    {
        public AsmDirective(List<Token> tokens)
            : base(tokens)
        {
        }
    }
}
