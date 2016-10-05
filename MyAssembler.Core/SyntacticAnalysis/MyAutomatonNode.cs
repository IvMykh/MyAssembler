using System;
using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;

namespace MyAssembler.Core.SyntacticAnalysis
{
    class MyAutomatonNode
        : IAutomatonNode
    {
        public List<Enum> PossibleInputs { get; private set; }
        public OperandsSetType OperandsSetType { get; private set; }
        public string ErrorMessageFormat { get; private set; }

        public List<MyAutomatonNode> Children { get; private set; }

        public MyAutomatonNode(
            List<Enum> possibleInputs, OperandsSetType operandsSetType, string errorMsgFormat)
        {
            PossibleInputs = possibleInputs;
            OperandsSetType = operandsSetType;
            ErrorMessageFormat = errorMsgFormat;

            Children = new List<MyAutomatonNode>();
        }

        public void AddChild(MyAutomatonNode child)
        {
            Children.Add(child);
        }
        public void AddChildren(params MyAutomatonNode[] children)
        {
            Children.AddRange(children);
        }

        private object getInputFromToken(Token currentToken)
        {
            switch (currentToken.Type)
            {
                case TokenType.Directive: return Enum.Parse(typeof(DirectiveType), currentToken.Value);
                case TokenType.Command:   return Enum.Parse(typeof(CommandType), currentToken.Value);

                default: return currentToken.Type;
            }
        }

        private LineParseResult passToChildren(IEnumerator<Token> enumerator)
        {
            Token currentToken = enumerator.Current;
            object input = getInputFromToken(currentToken);

            foreach (var child in Children)
            {
                if (child.PossibleInputs.Exists(entry => entry.Equals(input)))
                {
                    if (enumerator.MoveNext())
                    {
                        return child.passToChildren(enumerator);
                    }
                    else
                    {
                        var currPos = currentToken.Position;

                        return new LineParseResult(
                            new TokenPosition(currPos.Line, currPos.StartIndex + currentToken.Value.Length),
                            child.OperandsSetType,
                            child.ErrorMessageFormat);
                    }
                }
            }

            return new LineParseResult(currentToken.Position, OperandsSetType.NotAccepting, ErrorMessageFormat);
        }

        public LineParseResult TakeInput(IEnumerator<Token> enumerator)
        {
            if (PossibleInputs == null)
            {
                enumerator.MoveNext();
            }

            return passToChildren(enumerator);
        }
    }
}
