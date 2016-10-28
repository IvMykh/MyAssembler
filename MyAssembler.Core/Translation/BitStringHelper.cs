using System;

namespace MyAssembler.Core.Translation
{
    static class BitStringHelper
    {
        public static byte BitStringToByte(string bitString)
        {
            return Convert.ToByte(bitString, 2);
        }
    }
}
