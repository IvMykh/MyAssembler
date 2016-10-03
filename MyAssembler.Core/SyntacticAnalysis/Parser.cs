using System;
using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Properties;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core.SyntacticAnalysis
{
    public class Parser
    {
        private AsmCommand parseMov(List<Token> tokens, int commandTokenIndex)   { return null; }

        private AsmCommand parseAdd  (List<Token> tokens, int commandTokenIndex) { return null; }
        private AsmCommand parseSub  (List<Token> tokens, int commandTokenIndex) { return null; }
        private AsmCommand parseImul (List<Token> tokens, int commandTokenIndex) { return null; }
        private AsmCommand parseIdiv (List<Token> tokens, int commandTokenIndex) { return null; }
                                     
        private AsmCommand parseAnd  (List<Token> tokens, int commandTokenIndex) { return null; }
        private AsmCommand parseOr   (List<Token> tokens, int commandTokenIndex) { return null; }
        private AsmCommand parseNot  (List<Token> tokens, int commandTokenIndex) { return null; }
        private AsmCommand parseXor  (List<Token> tokens, int commandTokenIndex) { return null; }
                                     
        private AsmCommand parseJmp  (List<Token> tokens, int commandTokenIndex) { return null; }
        private AsmCommand parseJe   (List<Token> tokens, int commandTokenIndex) { return null; }
        private AsmCommand parseJne  (List<Token> tokens, int commandTokenIndex) { return null; }

        private AsmCommand parseCommand(List<Token> tokens, int commandTokenIndex)
        {
            Token commandToken = tokens[commandTokenIndex];

            CommandType commandType = CommandType.None;
            bool isParseSuccess = Enum.TryParse(commandToken.Value, out commandType);

            if (!isParseSuccess || commandType == CommandType.None)
            {
                throw new SyntacticalErrorException(
                    string.Format(Resources.CommandNotExistErrorMsgFormat,
                        commandToken.Value,
                        commandToken.Position.Line,
                        commandToken.Position.StartIndex));
            }
            else
            {
                switch (commandType)
                {
                    case CommandType.MOV:  return parseMov  (tokens, commandTokenIndex);
                    case CommandType.ADD:  return parseAdd  (tokens, commandTokenIndex);
                    case CommandType.SUB:  return parseSub  (tokens, commandTokenIndex);
                    case CommandType.IMUL: return parseImul (tokens, commandTokenIndex);
                    case CommandType.IDIV: return parseIdiv (tokens, commandTokenIndex);
                    case CommandType.AND:  return parseAnd  (tokens, commandTokenIndex);
                    case CommandType.OR:   return parseOr   (tokens, commandTokenIndex);
                    case CommandType.NOT:  return parseNot  (tokens, commandTokenIndex);
                    case CommandType.XOR:  return parseXor  (tokens, commandTokenIndex);
                    case CommandType.JMP:  return parseJmp  (tokens, commandTokenIndex);
                    case CommandType.JE:   return parseJe   (tokens, commandTokenIndex);
                    case CommandType.JNE:  return parseJne  (tokens, commandTokenIndex);

                    default: throw new SyntacticalErrorException(
                        string.Format(Resources.CommandNotImplementedErrorMsgFormat,
                            commandToken.Value,
                            commandToken.Position.Line,
                            commandToken.Position.StartIndex));
                }
            }
        }


        private AsmDirective parseDb (List<Token> tokens, int directiveTokenIndex) { return null; }
        private AsmDirective parseDw (List<Token> tokens, int directiveTokenIndex) { return null; }

        private AsmDirective parseDirective(List<Token> tokens, int directiveTokenIndex)
        {
            Token directiveToken = tokens[directiveTokenIndex];

            DirectiveType directiveType = DirectiveType.None;
            bool isParseSuccess = Enum.TryParse(directiveToken.Value, out directiveType);

            if (!isParseSuccess || directiveType == DirectiveType.None)
            {
                throw new SyntacticalErrorException(
                    string.Format(Resources.DirectiveNotExistErrorMsgFormat,
                        directiveToken.Value,
                        directiveToken.Position.Line,
                        directiveToken.Position.StartIndex));
            }
            else
            {
                switch (directiveType)
                {
                    case DirectiveType.DB: return parseDb (tokens, directiveTokenIndex);
                    case DirectiveType.DW: return parseDw (tokens, directiveTokenIndex);
                    
                    default: throw new SyntacticalErrorException(
                        string.Format(Resources.CommandNotImplementedErrorMsgFormat,
                            directiveToken.Value,
                            directiveToken.Position.Line,
                            directiveToken.Position.StartIndex));
                }
            }
        }

        public void Parse(List<List<Token>> tokensLists)
        {
            tokensLists.RemoveAll(tokens => tokens.Count == 0);

            var translationUnits = new List<AsmTranslationUnit>(tokensLists.Count);

            foreach (var tokensInLine in tokensLists)
            {
                int foundIndex = tokensInLine.FindIndex(
                    token => {
                        return token.Type == TokenType.Command ||
                               token.Type == TokenType.Directive;
                    });

                if (foundIndex == -1)
                {
                    throw new SyntacticalErrorException(
                        string.Format(Resources.IncorrectLineErrorMsgFormat,
                            tokensInLine[0].Position.Line));
                }

                switch (tokensInLine[foundIndex].Type)
                {
                    case TokenType.Directive:
                        {
                            translationUnits.Add(parseDirective(tokensInLine, foundIndex));
                        } break;
                    case TokenType.Command:
                        {
                            translationUnits.Add(parseCommand(tokensInLine, foundIndex));
                        } break;

                    default: throw new SyntacticalErrorException(
                        string.Format(Resources.UndefinedParseErrorMsgFormat,
                            tokensInLine[0].Position.Line));
                }
            }
        }
    }
}
