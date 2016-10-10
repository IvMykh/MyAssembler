using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public class AsmTranslationUnit
    {
        public List<Token> Tokens { get; protected set; }

        public AsmTranslationUnit(List<Token> tokens)
        {
            Tokens = tokens;
        }

        public virtual void Translate(TranslationContext context)
        {
            // ...
        }
    }
}
