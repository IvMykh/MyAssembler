using System.Collections.Generic;
using MyAssembler.Core.Translation.ParsersForConstants;

namespace MyAssembler.Core.Translation
{
    public class TranslationContext
    {
        private List<byte[]> _translatedBytesList;
     
        public RegisterBitsMapper RegisterBitsMapper { get; private set; }

        public BinConstantsParser BinConstsParser { get; private set; }
        public DecConstantsParser DecConstsParser { get; private set; }
        public HexConstantsParser HexConstsParser { get; private set; }

        public TranslationContext()
        {
            _translatedBytesList = new List<byte[]>();

            RegisterBitsMapper = new RegisterBitsMapper();

            BinConstsParser = new BinConstantsParser();
            DecConstsParser = new DecConstantsParser();
            HexConstsParser = new HexConstantsParser();
        }

        public void AddTranslatedUnit(byte[] translatedBytes)
        {
            _translatedBytesList.Add(translatedBytes);
        }
    }
}
