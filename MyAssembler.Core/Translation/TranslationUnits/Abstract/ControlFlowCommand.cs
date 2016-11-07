using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.OperandsTypeChecking;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class ControlFlowCommand
        : AsmCommand
    {
        public ControlFlowCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        protected abstract byte[] GetTranslatedBytes();

        protected override void TranslateCommand(TranslationContext context, int startPos)
        {
            Identifier identifier = null;
            context.Checker.CheckLabel(Tokens, startPos, out identifier);

            context.AddTranslatedUnit(GetTranslatedBytes());
        }

        protected short CalculateJumpValue(TranslationContext context, short targetAddress)
        {
            short currCommandLength = (short)context.TranslatedBytes[context.UnitCursor].Length;
            short currAddress = context.StartAddresses[context.UnitCursor];

            short jumpValue = (short)(targetAddress - currAddress - currCommandLength);

            return jumpValue;
        }
    }
}
