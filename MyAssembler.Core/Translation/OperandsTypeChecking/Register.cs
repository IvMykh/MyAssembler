using System;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Properties;

namespace MyAssembler.Core.Translation.OperandsTypeChecking
{
    public class Register
        : Operand
    {
        public RegisterType Type { get; private set; }

        public Register(string registerString)
        {
            Type = parse(registerString);
            W = wForRegister(Type);
        }

        private RegisterType parse(string registerString)
        {
            return (RegisterType)Enum.Parse(typeof(RegisterType), registerString);
        }
        private sbyte wForRegister(RegisterType register)
        {
            switch (register)
            {
                case RegisterType.AL:
                case RegisterType.AH:
                case RegisterType.BL:
                case RegisterType.BH:
                case RegisterType.CL:
                case RegisterType.CH:
                case RegisterType.DL:
                case RegisterType.DH: return WValueStore.ZERO;

                case RegisterType.AX:
                case RegisterType.BX:
                case RegisterType.CX:
                case RegisterType.DX:
                case RegisterType.SI:
                case RegisterType.DI:
                case RegisterType.SP:
                case RegisterType.BP: return WValueStore.ONE;

                default: throw new DesignErrorException(
                    string.Format(Resources.UnexpectedRegisterTypeMsgFormat, register));
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case RegisterType.AX: return Resources.AX;
                case RegisterType.AL: return Resources.AL;
                case RegisterType.AH: return Resources.AH;
                case RegisterType.BX: return Resources.BX;
                case RegisterType.BL: return Resources.BL;
                case RegisterType.BH: return Resources.BH;
                case RegisterType.CX: return Resources.CX;
                case RegisterType.CL: return Resources.CL;
                case RegisterType.CH: return Resources.CH;
                case RegisterType.DX: return Resources.DX;
                case RegisterType.DL: return Resources.DL;
                case RegisterType.DH: return Resources.DH;
                case RegisterType.SI: return Resources.SI;
                case RegisterType.DI: return Resources.DI;
                case RegisterType.SP: return Resources.SP;
                case RegisterType.BP: return Resources.BP;

                default: throw new DesignErrorException(
                    string.Format(Resources.UnexpectedRegisterTypeMsgFormat, Type));
            }
        }
    }
}
