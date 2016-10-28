using System;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Properties;

namespace MyAssembler.Core.Translation
{
    public class WValueHelper
    {
        public string WValueToString(WValue wValue)
        {
            return ((byte)wValue).ToString();
        }
        public WValue StringToWValue(string wValueString)
        {
            if (String.Compare(wValueString, Resources.W0) == 0)
            {
                return WValue.Zero;
            }
            else if (String.Compare(wValueString, Resources.W1) == 0)
            {
                return WValue.One;
            }

            throw new DesignErrorException(
                string.Format("'{0}': w bit can be either 0 or 1", 
                    wValueString));
        }
        public byte WValueToByte(WValue wValue)
        {
            return (byte)wValue;
        }

        public WValue WValueForRegister(RegisterType register)
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
                case RegisterType.DH: return WValue.Zero;

                case RegisterType.AX:
                case RegisterType.BX:
                case RegisterType.CX:
                case RegisterType.DX:
                case RegisterType.SI:
                case RegisterType.DI:
                case RegisterType.SP:
                case RegisterType.BP: return WValue.One;

                default: throw new DesignErrorException(
                    string.Format("'{0}': unexpected register type.",
                        register.ToString()));
            }
        }
    }
}
