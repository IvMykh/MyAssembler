using System;

namespace MyAssembler.Core.LexicalAnalysis
{
    public class LexicalErrorException
        : ApplicationException
    {
        public LexicalErrorException()
            : base()
        {
        }

        public LexicalErrorException(string message)
            : base(message)
        {
        }

        public LexicalErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
