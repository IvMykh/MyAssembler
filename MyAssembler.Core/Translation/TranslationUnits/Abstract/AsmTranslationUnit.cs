﻿using System;
using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;

namespace MyAssembler.Core.Translation.TranslationUnits.Abstract
{
    public abstract class AsmTranslationUnit
    {
        public List<Token> Tokens { get; protected set; }

        public AsmTranslationUnit(List<Token> tokens)
        {
            Tokens = tokens;
        }

        private void addLabelInfo(TranslationContext context)
        {
            context.MemoryManager.AddLabelInfo(Tokens[0].Value);
        }
        private void addMemCellInfo(TranslationContext context)
        {
            Token firstToken = Tokens[0];
            Token secondToken = Tokens[1];

            var dtvType = (DirectiveType)Enum.Parse(typeof(DirectiveType), secondToken.Value);

            switch (dtvType)
            {
                case DirectiveType.DB: 
                    context.MemoryManager.AddByteCell(firstToken.Value); break;
                case DirectiveType.DW: 
                    context.MemoryManager.AddWordCell(firstToken.Value); break;

                default: break;
            }
        }

        private void passIdentifiersInfoToContext(TranslationContext context)
        {
            Token firstToken = Tokens[0];
            Token secondToken = Tokens[1];

            if (firstToken.Type == TokenType.Identifier)
            {
                switch (secondToken.Type)
                {
                    case TokenType.Colon: addLabelInfo(context); break;
                    case TokenType.Directive: addMemCellInfo(context); break;

                    default: throw new DesignErrorException();
                }
            }
        }
        protected virtual void Translate(TranslationContext context)
        {
            // TODO: make abstract in the end.
        }

        public void Accept(TranslationContext context)
        {
            switch (context.AcceptMode)
            {
                case ContextAcceptMode.CollectIdentifiersMode:
                    {
                        passIdentifiersInfoToContext(context);
                    } break;
                case ContextAcceptMode.TranslateMode:
                    {
                        Translate(context);
                    } break;
                case ContextAcceptMode.InsertAddressMode:
                    {
                        // So far...
                        throw new NotImplementedException();
                    } // break;
                
                default: throw new TranslationErrorException(
                    string.Format("{0}: translation mode is not supported",
                        context.AcceptMode));
            }
        }
    }
}
