﻿using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    public class XorCommand
        : FiveCasesCommand
    {
        public XorCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        protected override string RegRegFormat  
        {
            get { 
                return "0011001{0}"; 
            }
        }
        protected override string RegMemFormat  
        {
            get { 
                return "0011001{0}"; 
            }
        }
        protected override string MemRegFormat  
        {
            get { 
                return "0011000{0}"; 
            }
        }
        protected override string RegImFormat   
        {
            get { 
                return "100000{0}{1}"; 
            }
        }
        protected override string MemImFormat   
        {
            get { 
                return "100000{0}{1}"; 
            }
        }

        protected override string RegFieldForIm 
        {
            get { 
                return "110"; 
            }
        }
    }
}
