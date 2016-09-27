using System.Collections.Generic;

namespace MyAssembler.Core.LexicalAnalysis
{
    public interface ITokenDefinitionsStore
    {
        IEnumerable<TokenDefinition> GetTokenDefinitions();
    }
}
