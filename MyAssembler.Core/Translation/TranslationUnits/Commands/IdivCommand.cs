using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    // idiv <reg32>
    // idiv <mem> 
    public class IdivCommand
        : SingleOperandCommand
    {

        public IdivCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }
        
        protected override string OperationCodeFormat  
        { 
            get { 
                return "1111011{0}"; } 
        }
        protected override string AddressingByteFormat 
        {
            get {
                return "{0}111{1}";
            }
        }
    }
}
