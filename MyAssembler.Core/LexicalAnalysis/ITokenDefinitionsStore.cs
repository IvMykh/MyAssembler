using System.Collections.Generic;

namespace MyAssembler.Core.LexicalAnalysis
{
    public interface ITokenDefinitionsStore
    {
        List<TokenDefinition> GetTokenDefinitions();
    }
}
