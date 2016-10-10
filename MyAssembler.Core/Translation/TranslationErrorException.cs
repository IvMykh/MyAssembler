using System;

namespace MyAssembler.Core.Translation
{
    public class TranslationErrorException
        : CompilationErrorException
    {
        public TranslationErrorException()
            : base()
        {
        }

        public TranslationErrorException(string message)
            : base(message)
        {
        }

        public TranslationErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
