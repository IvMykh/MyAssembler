using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    /*
     * ( R/M ) -> ( R )
     * ( R )   -> ( R/M )
     * ( R/M ) -> ( im )
     * ( R )   -> ( im )
     */
    public class MovCommand
        : AsmCommand
    {
        public MovCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }
    }
}
