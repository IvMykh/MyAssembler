using System;

namespace MyAssembler.Core.SyntacticAnalysis
{
    public class SyntacticalErrorException
        : CompilationErrorException
    {
        public SyntacticalErrorException()
            : base()
        {
        }

        public SyntacticalErrorException(string message)
            : base(message)
        {
        }

        public SyntacticalErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
