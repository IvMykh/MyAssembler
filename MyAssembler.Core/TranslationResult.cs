using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;

namespace MyAssembler.Core
{
    public struct TranslationResult
    {
        public IReadOnlyList<IReadOnlyList<Token>> TokensLists { get; set; }
        public IReadOnlyList<byte[]> TranslatedBytes { get; set; }
        public IReadOnlyList<short> Addresses { get; set; }

        public IReadOnlyDictionary<string, short> Labels { get; set; }
        public IReadOnlyDictionary<string, short> ByteCells { get; set; }
        public IReadOnlyDictionary<string, short> WordCells { get; set; }
    }
}
