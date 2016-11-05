using System;
using System.Globalization;
using MyAssembler.Core.Properties;

namespace MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants
{
    public abstract class ConstantsParser
    {
        public abstract byte[] Parse(string value);

        protected byte[] ParseByTypeParseMethod(
            string value, NumberStyles numberStyles, params char[] trimChars)
        {
            string clearedValue = value.TrimEnd(trimChars);

            short parsedValue = 0;

            try
            {
                parsedValue = short.Parse(clearedValue, numberStyles);
            }
            catch (OverflowException)
            {
                throw new TranslationErrorException(
                    string.Format(Resources.ConstantOverflowMsgFormat, value));
            }

            byte[] bytes = BitConverter.GetBytes(parsedValue);

            if (bytes[1] == 0)
            {
                return new byte[] { bytes[0] };
            }

            return bytes;
        }
    }
}
