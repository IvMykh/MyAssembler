using System;

namespace MyAssembler.Core.Translation
{
    class DesignErrorException
        : CompilationErrorException
    {
        public DesignErrorException()
            : base()
        {
        }

        public DesignErrorException(string message)
            : base(message)
        {
        }

        public DesignErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
