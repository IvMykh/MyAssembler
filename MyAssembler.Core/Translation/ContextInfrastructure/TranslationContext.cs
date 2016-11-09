using System.Collections.Generic;
using MyAssembler.Core.Translation.OperandsTypeChecking;

namespace MyAssembler.Core.Translation.ContextInfrastructure
{
    public class TranslationContext
    {
        public const short INIT_OFFSET = 0x100;

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
        
        private ContextAcceptMode _acceptMode;
        
        public ContextAcceptMode AcceptMode 
        { 
            get { 
                return _acceptMode; 
            } 
            set { 
                _acceptMode = value;

                if (value == ContextAcceptMode.InsertAddressMode)
                    UnitCursor = 0;
                else
                    UnitCursor = -1;
            }
        }

        public int UnitCursor { get; set; }

        public TypeChecker Checker { get; private set; }
        public IMemoryManager MemoryManager { get; private set; }



        public TranslationContext(IMemoryManager memoryManager)
        {
            _translatedBytesList = new List<byte[]>();
            _startAddresses = new List<short>();

            // Initial mode.
            AcceptMode = ContextAcceptMode.CollectIdentifiersMode; 

            Checker = new TypeChecker(this);
            MemoryManager = memoryManager;
        }

        public void AddTranslatedUnit(byte[] translatedBytes)
        {
            _translatedBytesList.Add(translatedBytes);
            ++UnitCursor;

            if (UnitCursor == 0)
            {
                _startAddresses.Add(INIT_OFFSET);
            }
            else
            {
                int startAddress =
                    _startAddresses[UnitCursor - 1] +
                    _translatedBytesList[UnitCursor - 1].Length;
                
                _startAddresses.Add((short)startAddress);
            }
        }
        
        public void InsertAddressValue(int addrStartPos, short addrValue)
        {
            byte upperByte = (byte)(addrValue >> 8);
            byte lowerByte = (byte)(addrValue & 0xff);

            byte exchangeVal = upperByte;
            upperByte = lowerByte;
            lowerByte = exchangeVal;

            _translatedBytesList[UnitCursor][addrStartPos]     = upperByte;
            _translatedBytesList[UnitCursor][addrStartPos + 1] = lowerByte;
        }
    }
}
