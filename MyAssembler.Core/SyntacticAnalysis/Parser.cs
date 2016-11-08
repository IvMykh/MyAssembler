using System;
using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Properties;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;
using MyAssembler.Core.Translation.TranslationUnits.Commands;
using MyAssembler.Core.Translation.TranslationUnits.Directives;

namespace MyAssembler.Core.SyntacticAnalysis
{
    /// <summary>
    /// Analyzes whether lines of tokens constitute valid assembly statements 
    /// and produces translation units.
    /// </summary>
    public class Parser
    {
        private IAutomatonNode _automaton;

        public Parser(IAutomatonBuilder automatonBuilder)
        {
            automatonBuilder.Construct();
            _automaton = automatonBuilder.ConstructedInstance;
        }

        private AsmCommand createAsmCommand(Token definitionToken, List<Token> tokens, OperandsSetType ost)
        {
            CommandType commandType = 
                (CommandType)Enum.Parse(typeof(CommandType), definitionToken.Value);

            switch (commandType)
            {
                case CommandType.MOV:  return new MovCommand(tokens, ost);
                case CommandType.ADD:  return new AddCommand(tokens, ost);
                case CommandType.SUB:  return new SubCommand(tokens, ost);
                case CommandType.IMUL: return new ImulCommand(tokens, ost);
                case CommandType.IDIV: return new IdivCommand(tokens, ost);
                case CommandType.AND:  return new AndCommand(tokens, ost);
                case CommandType.OR:   return new OrCommand(tokens, ost);
                case CommandType.NOT:  return new NotCommand(tokens, ost);
                case CommandType.XOR:  return new XorCommand(tokens, ost);
                case CommandType.JMP:  return new JmpCommand(tokens, ost);
                case CommandType.JE:   return new JeCommand(tokens, ost);
                case CommandType.JNE:  return new JneCommand(tokens, ost);
                case CommandType.INT:  return new IntCommand(tokens, ost);

                default: throw new SyntacticalErrorException(
                    string.Format(Resources.CmdNotImplementedMsgFormat,
                        definitionToken.Value,
                        definitionToken.Position.Line,
                        definitionToken.Position.StartIndex));
            }
        }
        private AsmDirective createAsmDirective(Token definitionToken, List<Token> tokens)
        {
            DirectiveType directiveType =
                (DirectiveType)Enum.Parse(typeof(DirectiveType), definitionToken.Value);

            switch (directiveType)
            {
                case DirectiveType.DB: return new DbDirective(tokens);
                case DirectiveType.DW: return new DwDirective(tokens);

                default: throw new SyntacticalErrorException(
                    string.Format(Resources.DtvNotImplementedMsgFormat,
                        definitionToken.Value,
                        definitionToken.Position.Line,
                        definitionToken.Position.StartIndex));
            }
        }
        private AsmTranslationUnit createTranslationUnit(List<Token> tokens, OperandsSetType ost)
        {
            Token definitionToken = tokens.Find(
                token => { 
                    return token.Type == TokenType.Command || 
                           token.Type == TokenType.Directive; 
                });

            TokenType translationUnitType = definitionToken.Type;

            switch (translationUnitType)
            {
                case TokenType.Command:   return createAsmCommand(definitionToken, tokens, ost);
                case TokenType.Directive: return createAsmDirective(definitionToken, tokens);

                default: throw new Exception("Absolutely impossible exception!");
            }
        }

        private LineParseResult parseLine(List<Token> tokens)
        {
            return _automaton.TakeInput(tokens.GetEnumerator());
        }
        
        public List<AsmTranslationUnit> Parse(List<List<Token>> tokensLists)
        {
            tokensLists.RemoveAll(tokens => tokens.Count == 0);

            var translationUnits = new List<AsmTranslationUnit>(tokensLists.Count);

            foreach (var tokensInLine in tokensLists)
            {
                var parseResult = parseLine(tokensInLine);

                if (parseResult.OperandsSetType != OperandsSetType.NotAccepting)
                {
                    translationUnits.Add(
                        createTranslationUnit(tokensInLine, parseResult.OperandsSetType));
                }
                else
                {
                    throw new SyntacticalErrorException(
                        string.Format(parseResult.ErrorMsgFormat,
                            parseResult.Position.Line,
                            parseResult.Position.StartIndex));
                }
            }

            return translationUnits;
        }
    }
}
