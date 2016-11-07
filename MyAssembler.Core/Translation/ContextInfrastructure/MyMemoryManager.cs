using System;
using System.Collections.Generic;
using MyAssembler.Core.Properties;

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
            get {
                return _labels;
            }
        }

        public void AddByteCell(string byteCellName, short address = 0)
        {
            try
            {
                _byteCells.Add(byteCellName, address);
            }
            catch (ArgumentException)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.ByteCellRedefinitionMsgFormat, byteCellName));
            }
        }
        public void AddWordCell(string wordCellName, short address = 0)
        {
            try
            {
                _wordCells.Add(wordCellName, address);
            }
            catch (ArgumentException)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.WordCellRedefinitionMsgFormat, wordCellName));
            }
        }
        public void AddLabelInfo(string labelName, short address = 0)
        {
            try
            {
                _labels.Add(labelName, address);
            }
            catch (ArgumentException)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.LabelRedefinitionMsgFormat, labelName));
            }
        }


        public void InsertByteCellAddress(string byteCellName, short address)
        {
            _byteCells[byteCellName] = address;
        }
        public void InsertWordCellAddress(string wordCellName, short address)
        {
            _wordCells[wordCellName] = address;
        }
        public void InsertLabelAddress(string labelName, short address)
        {
            _labels[labelName] = address;
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
                string.Format(Resources.IdfNotCollectedMsgFormat, identifier));
        }
        public short GetAddressFor(string identifier)
        {
            if (_byteCells.ContainsKey(identifier))
            {
                return _byteCells[identifier];
            }
            else if (_wordCells.ContainsKey(identifier))
            {
                return _wordCells[identifier];
            }
            else if (_labels.ContainsKey(identifier))
            {
                return _labels[identifier];
            }

            throw new DesignErrorException(
                string.Format(Resources.IdfNotCollectedMsgFormat, identifier));
        }
    }
}
