using System.Globalization;

namespace MyAssembler.Core.Translation.ParsersForConstants
{
    /// <summary>
    /// Returns array of bytes of the given decimal value represented as string (in reverse order).
    /// </summary>
    public class DecConstantsParser
        : ConstantsParser
    {
        public override byte[] Parse(string value)
        {
            return ParseByTypeParseMethod(value, NumberStyles.Integer, 'd', 'D');
        }
    }
}
