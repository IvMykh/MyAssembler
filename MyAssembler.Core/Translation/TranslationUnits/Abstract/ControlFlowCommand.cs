using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;

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

        protected override void Translate(TranslationContext context)
        {
            int i = 1;
            if (Tokens[0].Type == TokenType.Identifier)
            {
                // Skip label and ':' tokens.
                ++i;
                ++i;
            }

            string identifier = Tokens[i].Value;
            IdentifierType idType = context.MemoryManager.GetIdentifierType(identifier);

            if (idType != IdentifierType.Label)
            {
                throw new TranslationErrorException(
                    string.Format("'{0}': the identifier is not a label.",
                        identifier));
            }

            context.AddTranslatedUnit(GetTranslatedBytes());
        }
    }
}
