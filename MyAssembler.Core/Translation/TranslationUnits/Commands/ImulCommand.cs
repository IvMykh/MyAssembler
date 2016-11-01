using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    // imul <reg32>,<reg32>
    // imul <reg32>,<mem>
    // imul <reg32>,<reg32>,<con>
    // imul <reg32>,<mem>,<con>
    public class ImulCommand
        : AsmCommand
    {
        public ImulCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }
    }
}
