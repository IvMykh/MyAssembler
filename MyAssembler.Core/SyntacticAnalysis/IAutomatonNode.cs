using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;

namespace MyAssembler.Core.SyntacticAnalysis
{
    public interface IAutomatonNode
    {
        LineParseResult TakeInput(IEnumerator<Token> enumerator);
    }
}
