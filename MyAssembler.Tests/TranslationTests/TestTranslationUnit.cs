using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Tests.TranslationTests
{
    /// <summary>
    /// For testing purposes only.
    /// </summary>
    class TestTranslationUnit
        : AsmTranslationUnit
    {
        public TestTranslationUnit(List<Token> tokens)
            : base(tokens)
        {
        }

        protected override void Translate(TranslationContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
