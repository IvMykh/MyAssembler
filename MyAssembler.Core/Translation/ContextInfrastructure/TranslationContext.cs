using System.Collections.Generic;
using MyAssembler.Core.Translation.OperandsTypeChecking;

namespace MyAssembler.Core.Translation.ContextInfrastructure
{
    public class TranslationContext
    {
        private List<byte[]> _translatedBytes;

        public IReadOnlyList<byte[]> TranslatedBytes
        {
            get {
                return _translatedBytes;
            }
        }

        public TypeChecker Checker { get; private set; }
        public IMemoryManager MemoryManager { get; private set; }
        
        public ContextAcceptMode AcceptMode { get; set; }

        public TranslationContext(IMemoryManager memoryManager)
        {
            _translatedBytes = new List<byte[]>();

            Checker = new TypeChecker(this);
            MemoryManager = memoryManager;

            // Initial mode.
            AcceptMode = ContextAcceptMode.CollectIdentifiersMode;
        }

        public void AddTranslatedUnit(byte[] translatedBytes)
        {
            _translatedBytes.Add(translatedBytes);
        }
    }
}
