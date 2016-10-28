using System.Collections.Generic;

namespace MyAssembler.Core.Translation.ContextInfrastructure
{
    public class MyMemoryManager
        : IMemoryManager
    {
        private Dictionary<string, short> _byteCells;
        private Dictionary<string, short> _wordCells;
        private Dictionary<string, short> _labels;

        public MyMemoryManager()
        {
            _byteCells = new Dictionary<string, short>();
            _wordCells = new Dictionary<string, short>();
            _labels = new Dictionary<string, short>();
        }

        public IReadOnlyDictionary<string, short> ByteCells
        { 
            get { 
                return _byteCells; 
            } 
        }
        public IReadOnlyDictionary<string, short> WordCells
        { 
            get { 
                return _wordCells; 
            } 
        }
        public IReadOnlyDictionary<string, short> Labels
        {
            get
            {
                return _labels;
            }
        }

        public void AddByteCell(string byteCellName, short address = 0)
        {
            _byteCells.Add(byteCellName, address);
        }
        public void AddWordCell(string wordCellName, short address = 0)
        {
            _wordCells.Add(wordCellName, address);
        }
        public void AddLabelInfo(string labelName, short address = 0)
        {
            _labels.Add(labelName, address);
        }

        public IdentifierType GetIdentifierType(string identifier)
        {
            if (_byteCells.ContainsKey(identifier))
            {
                return IdentifierType.Byte;
            }
            else if (_wordCells.ContainsKey(identifier))
            {
                return IdentifierType.Word;
            }
            else if (_labels.ContainsKey(identifier))
            {
                return IdentifierType.Label;
            }

            throw new DesignErrorException(
                string.Format("'{0}': identifier was not collected at the previous pass.",
                    identifier));
        }
    }
}
