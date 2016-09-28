using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MyAssembler.Core.Properties;

namespace MyAssembler.Core.LexicalAnalysis
{    
    public class Lexer
    {
        private List<TokenDefinition>   _tokenDefinitions;

        public Lexer(ITokenDefinitionsStore store)
        {
            _tokenDefinitions = store.GetTokenDefinitions();
        }

        private void prepareCode(List<string> lines)
        {
            var commentStartSymbol = char.Parse(Resources.CommentStartSymbol);

            for (int i = 0; i < lines.Count; i++)
            {
                int commentStartPos = lines[i].IndexOf(commentStartSymbol);

                if (commentStartPos > -1)
                {
                    lines[i] = lines[i].Remove(commentStartPos)
                                       .TrimEnd();
                }
            }
        }

        public List<List<Token>> Tokenize(List<string> linesOfCode)
        {
            prepareCode(linesOfCode);

            int linesCount = linesOfCode.Count();
            var tokensLists = new List<List<Token>>(linesCount);

            int currLine = 0;
            foreach (var line in linesOfCode)
            {
                tokensLists.Add(new List<Token>());

                string remainingLine = line.TrimStart();
                int currIndex = line.Length - remainingLine.Length;

                while (remainingLine.Length > 0)
                {
                    var bestMatch = (from tokenDef in _tokenDefinitions
                                    let regex = new Regex(tokenDef.Regex, RegexOptions.IgnoreCase)
                                    let match = regex.Match(remainingLine)
                                    orderby match.Length descending, tokenDef.Type descending
                                    select new { 
                                        TokenType = tokenDef.Type, 
                                        Value = match.Value 
                                    }).First();

                    if (bestMatch.Value.Length == 0)
                    {
                        throw new LexicalErrorException(string.Format(Resources.LexicalErrorFormat,
                            remainingLine.First(),
                            currLine + 1,
                            currIndex + 1));
                    }

                    tokensLists[currLine].Add(
                        new Token(bestMatch.TokenType, bestMatch.Value, new TokenPosition(currLine, currIndex)));

                    string nextRemainingLine = remainingLine.Remove(0, bestMatch.Value.Length)
                                                            .TrimStart();
                    currIndex += (remainingLine.Length - nextRemainingLine.Length);
                    remainingLine = nextRemainingLine;
                }
                
                ++currLine;
            }

            return tokensLists;
        }
    }
}
