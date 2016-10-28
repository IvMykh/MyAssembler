namespace MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants
{ 
    /// <summary>
    /// Returns array of bytes of the given binary value represented as string (in reverse order).
    /// </summary>
    public class BinConstantsParser
        : ConstantsParser
    {
        private const int BITS_IN_BYTE = 8;

        public override byte[] Parse(string value)
        {
            string clearedValue = value.TrimStart('0')
                                       .TrimEnd('b', 'B');

            if (clearedValue.Length == 0)
            {
                return new byte[] { 0 };
            }

            int remainingBitsCount = clearedValue.Length % BITS_IN_BYTE;
            int bytesCount = clearedValue.Length / BITS_IN_BYTE +
                            ((remainingBitsCount != 0) ? 1 : 0);

            if (bytesCount > 2)
            {
                throw new TranslationErrorException(
                    string.Format("{0}: value overflow.", 
                        value));
            }

            byte[] bytes = new byte[bytesCount];
            int endPos = clearedValue.Length;

            for (int i = 0; i < bytes.Length - 1; ++i)
            {
                var bitString = clearedValue.Substring(endPos - BITS_IN_BYTE, BITS_IN_BYTE);
                bytes[i] = BitStringHelper.BitStringToByte(bitString);

                endPos -= BITS_IN_BYTE;
            }

            int countToSubstr = (remainingBitsCount == 0) ? BITS_IN_BYTE : remainingBitsCount;
            string lastBitString = clearedValue.Substring(0, countToSubstr);

            bytes[bytes.Length - 1] = BitStringHelper.BitStringToByte(lastBitString);

            return bytes;
        }
    }
}
