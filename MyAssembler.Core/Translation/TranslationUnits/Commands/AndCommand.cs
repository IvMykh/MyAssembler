﻿using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Commands
{
    public class AndCommand
        : AsmCommand
    {
        public AndCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }
    }
}