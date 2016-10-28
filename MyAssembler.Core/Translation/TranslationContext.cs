using System.Collections.Generic;
using MyAssembler.Core.Translation.ParsersForConstants;

namespace MyAssembler.Core.Translation
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

        public IMemoryManager MemoryManager { get; private set; }

        public WValueHelper WValueHelper{ get; private set; }
        public RegisterHelper RegisterHelper { get; private set; }

        public BinConstantsParser BinConstsParser { get; private set; }
        public DecConstantsParser DecConstsParser { get; private set; }
        public HexConstantsParser HexConstsParser { get; private set; }
        
        public ContextAcceptMode AcceptMode { get; set; }

        public TranslationContext(IMemoryManager memoryManager)
        {
            _translatedBytes = new List<byte[]>();

            MemoryManager = memoryManager;

            WValueHelper = new WValueHelper();
            RegisterHelper = new RegisterHelper();

            BinConstsParser = new BinConstantsParser();
            DecConstsParser = new DecConstantsParser();
            HexConstsParser = new HexConstantsParser();

            // Initial mode.
            AcceptMode = ContextAcceptMode.CollectIdentifiersMode;
        }

        public void AddTranslatedUnit(byte[] translatedBytes)
        {
            _translatedBytes.Add(translatedBytes);
        }
    }
}
