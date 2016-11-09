using System.Text;

namespace MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants
{
    public class LiteralParser
        : ConstantsParser
    {
        public override byte[] Parse(string value)
        {
            return Encoding.ASCII.GetBytes
                (value.Substring(1, value.Length - 2) // remove open and closing quote.
                      .ToCharArray());
        }
    }
}
