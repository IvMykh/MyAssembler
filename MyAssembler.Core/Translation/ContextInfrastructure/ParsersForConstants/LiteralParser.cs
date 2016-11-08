using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants
{
    public class LiteralParser
        : ConstantsParser
    {
        public override byte[] Parse(string value)
        {
            return Encoding.ASCII.GetBytes(value.ToCharArray());
        }
    }
}
