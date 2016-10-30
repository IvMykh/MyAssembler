using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class ConditionalJumpCommand
        : AsmCommand
    {
        public ConditionalJumpCommand(List<Token> tokens, OperandsSetType operandsSetType)
            : base(tokens, operandsSetType)
        {
        }

        protected abstract string SecondBytePlaceholder { get; }

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

            int bytesCount = 4;
            byte[] translatedBytes = new byte[bytesCount];

            translatedBytes[0] = BitStringHelper.BitStringToByte("00001111");
            translatedBytes[1] = BitStringHelper.BitStringToByte(
                string.Format("1000{0}",
                    SecondBytePlaceholder));

            context.AddTranslatedUnit(translatedBytes);
        }
    }
}
