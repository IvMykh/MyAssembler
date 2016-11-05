using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    public class NotCommand
        : SingleOperandCommand
    {
        public NotCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        protected override string OperationCodeFormat  
        {
            get { 
                return "1111011{0}"; 
            }
        }
        protected override string AddressingByteFormat 
        {
            get { 
                return "{0}010{1}"; 
            }
        }
    }
}
