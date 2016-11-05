using System.Collections.Generic;
using MyAssembler.Core.Translation.OperandsTypeChecking;

namespace MyAssembler.Core.Translation.ContextInfrastructure
{
    public class TranslationContext
    {
        private const short INIT_OFFSET = 100;

        private List<byte[]> _translatedBytesList;
        private List<short>  _startAddresses;

        public IReadOnlyList<byte[]> TranslatedBytes
        {
            get {
                return _translatedBytesList;
            }
        }

        public IReadOnlyList<short> StartAddresses
        {
            get {
                return _startAddresses;
            }
        }

        public TypeChecker Checker { get; private set; }
        public IMemoryManager MemoryManager { get; private set; }
        
        public ContextAcceptMode AcceptMode { get; set; }

        public TranslationContext(IMemoryManager memoryManager)
        {
            _translatedBytesList = new List<byte[]>();
            _startAddresses = new List<short>();

            Checker = new TypeChecker(this);
            MemoryManager = memoryManager;

            // Initial mode.
            AcceptMode = ContextAcceptMode.CollectIdentifiersMode;
        }

        public void AddTranslatedUnit(byte[] translatedBytes)
        {
            _translatedBytesList.Add(translatedBytes);

            if (_translatedBytesList.Count == 1)
            {
                _startAddresses.Add(INIT_OFFSET);
            }
            else
            {
                int startAddress = 
                    _startAddresses[_startAddresses.Count - 1] + 
                    translatedBytes.Length; 
                
                _startAddresses.Add((short)startAddress);
            }
        }
    }
}
