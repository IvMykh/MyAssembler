using System;

namespace MyAssembler.Core.Translation
{
    static class BitStringManipHelper
    {
        public static byte BitStringToByte(string bitString)
        {
            return Convert.ToByte(bitString, 2);
        }
    }
}
