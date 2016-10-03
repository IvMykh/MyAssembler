using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;


namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class AsmCommand
        : AsmTranslationUnit
    {
        public AsmCommand(List<Token> tokens)
            : base(tokens)
        {
        }
    }
}
