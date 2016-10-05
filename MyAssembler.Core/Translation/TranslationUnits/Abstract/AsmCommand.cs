using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class AsmCommand
        : AsmTranslationUnit
    {
        public OperandsSetType OperandsSetType { get; protected set; }

        public AsmCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens)
        {
            OperandsSetType = operandsSetType;
        }
    }
}
