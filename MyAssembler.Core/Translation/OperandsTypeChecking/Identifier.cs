using MyAssembler.Core.Properties;
using MyAssembler.Core.Translation.ContextInfrastructure;

namespace MyAssembler.Core.Translation.OperandsTypeChecking
{
    public class Identifier
        : Operand
    {
        public string Name { get; private set; }
        public IdentifierType Type { get; private set; }

        public Identifier(string name, IdentifierType type)
        {
            Name = name;
            Type = type;

            switch (Type)
            {
                case IdentifierType.Byte: W = WValueStore.ZERO; break;
                case IdentifierType.Word: W = WValueStore.ONE; break;
                case IdentifierType.Label: W = WValueStore.NO_VALUE; break;

                default: throw new DesignErrorException(
                    string.Format(Resources.IdfTypeNotSupportedMsgFormat, Type));
            }
        }
    }
}
