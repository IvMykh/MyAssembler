using System;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Properties;

namespace MyAssembler.Core.Translation
{
    public class RegisterHelper
    {
        public string RegisterToBitString(RegisterType register)
        {
            switch (register)
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
                    string.Format("'{0}': unexpected register type.", 
                        register.ToString()));
            }
        }
        
        public RegisterType Parse(string registerString)
        {
            return (RegisterType)Enum.Parse(typeof(RegisterType), registerString);
        }
    }
}
