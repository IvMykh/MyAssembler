using System.Collections.Generic;

namespace MyAssembler.Core.Translation.ContextInfrastructure
{
    public interface IMemoryManager
    {
        void AddByteCell(string byteCellName, short address = 0);
        void AddWordCell(string wordCellName, short address = 0);
        void AddLabelInfo(string labelName, short address = 0);

        IReadOnlyDictionary<string, short> ByteCells { get; }
        IReadOnlyDictionary<string, short> WordCells { get; }
        IReadOnlyDictionary<string, short> Labels    { get; }

        void InsertByteCellAddress(string byteCellName, short address);
        void InsertWordCellAddress(string wordCellName, short address);
        void InsertLabelAddress(string labelName, short address);

        IdentifierType GetIdentifierType(string identifier);
        short GetAddressFor(string identifier);
    }
}
