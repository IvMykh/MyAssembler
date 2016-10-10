using System;
using MyAssembler.Core.LexicalAnalysis;

namespace MyAssembler.Core.Translation
{
    public struct RegisterBitsMapResult
    {
        public string BitsString { get; set; }
        public string W { get; set; }

        public RegisterBitsMapResult(string bitsString, string w)
            : this()
        {
            BitsString = bitsString;
            W = w;
        }
    }

    public class RegisterBitsMapper
    {
        private RegisterBitsMapResult getResult(string bitsString, string w)
        {
            return new RegisterBitsMapResult(bitsString, w);
        }

        public byte WStringToByte(string w)
        {
            switch (w)
            {
                case "0": return 0;
                case "1": return 1;
                
                default: throw new ArgumentException(
                    string.Format("{0} in WStringToByte: argument is invalid", w));
            }
        }

        public RegisterBitsMapResult Map(RegisterType registerType)
        {
            switch (registerType)
            {
                case RegisterType.AL: return getResult("000", "0");
                case RegisterType.CL: return getResult("001", "0");
                case RegisterType.DL: return getResult("010", "0");
                case RegisterType.BL: return getResult("011", "0");

                case RegisterType.AH: return getResult("100", "0");
                case RegisterType.CH: return getResult("101", "0");
                case RegisterType.DH: return getResult("110", "0");
                case RegisterType.BH: return getResult("111", "0");

                case RegisterType.AX: return getResult("000", "1");
                case RegisterType.CX: return getResult("001", "1");
                case RegisterType.DX: return getResult("010", "1");
                case RegisterType.BX: return getResult("011", "1");

                case RegisterType.SP: return getResult("100", "1");
                case RegisterType.BP: return getResult("101", "1");
                case RegisterType.SI: return getResult("110", "1");
                case RegisterType.DI: return getResult("111", "1");

                default: throw new TranslationErrorException(
                    string.Format("{0}: register is not supported.", 
                        registerType));
            }
        }
    }
}
