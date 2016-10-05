﻿using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.Translation.TranslationUnits.Directives
{
    public class DwDirective
        : AsmDirective
    {
        public DwDirective(List<Token> tokens)
            : base(tokens)
        {
        }
    }
}
