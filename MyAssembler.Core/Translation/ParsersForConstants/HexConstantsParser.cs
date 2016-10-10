using System.Globalization;

namespace MyAssembler.Core.Translation.ParsersForConstants
{
    /// <summary>
    /// Returns array of bytes of the given hexadecimal value represented as string (in reverse order).
    /// </summary>
    public class HexConstantsParser
        : ConstantsParser
    {
        public override byte[] Parse(string value)
        {
            return ParseByTypeParseMethod(value, NumberStyles.HexNumber, 'h', 'H');
        }
    }
}
