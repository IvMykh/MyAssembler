using System;

namespace MyAssembler.Core
{
    public class CompilationErrorException
        : ApplicationException
    {
        public CompilationErrorException()
            : base()
        {
        }

        public CompilationErrorException(string message)
            : base(message)
        {
        }

        public CompilationErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
