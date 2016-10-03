using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Tests.Properties;

namespace MyAssembler.Tests
{
    [TestClass]
    public class LexerTests
    {
        private Lexer _testedInstance = new Lexer(new MyAsmTokenDefinitionsStore());

        [TestMethod]
        public void TestListsCount()
        {
            // Arrange.
            var strings = new List<string> 
                { 
                    string.Empty, 
                    string.Empty, 
                    string.Empty 
                };

            // Act.
            var tokenslists = _testedInstance.Tokenize(strings);

            // Assert.
            Assert.AreEqual(tokenslists.Count(), strings.Count);
        }

        private void runTest(string sampleString, List<Token> expectedTokens)
        {
            // Arrange.
            var sampleStrings = new List<string> { sampleString };

            // Act.
            var tokensLists = _testedInstance.Tokenize(sampleStrings);

            // Assert.
            Assert.AreEqual(1, tokensLists.Count());
            var tokens = tokensLists.First();

            Assert.AreEqual(expectedTokens.Count, tokens.Count());

            int j = 0;
            foreach (var token in tokens)
            {
                Assert.AreEqual(expectedTokens[j].Type, token.Type);
                Assert.AreEqual(expectedTokens[j].Value, token.Value);
                Assert.AreEqual(expectedTokens[j].Position, token.Position);

                ++j;
            }
        }

        [TestMethod]
        public void TestSpecialSymbolsTokenization()
        {
            var sampleString = ",:[]+?";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.Comma,              ",", new TokenPosition(0, 0)),
                    new Token(TokenType.Colon,              ":", new TokenPosition(0, 1)),
                    new Token(TokenType.OpenSquareBracket,  "[", new TokenPosition(0, 2)),
                    new Token(TokenType.CloseSquareBracket, "]", new TokenPosition(0, 3)),
                    new Token(TokenType.Plus,               "+", new TokenPosition(0, 4)),
                    new Token(TokenType.QuestionMark,       "?", new TokenPosition(0, 5))
                };

            runTest(sampleString, expectedTokens);
        }

        [TestMethod]
        public void TestDirectivesTokenization()
        {
            var sampleString = "DW db  ";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.Directive, "DW",  new TokenPosition(0, 0)),
                    new Token(TokenType.Directive, "DB",  new TokenPosition(0, 3))
                };

            runTest(sampleString, expectedTokens);
        }

        [TestMethod]
        public void TestCommandsTokenization()
        {
            var sampleString = "MOV add sUb IMUL IDIV AND OR NOT XOR JMP JE JNE";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.Command, "MOV",  new TokenPosition(0, 0)),
                    new Token(TokenType.Command, "ADD",  new TokenPosition(0, 4)),
                    new Token(TokenType.Command, "SUB",  new TokenPosition(0, 8)),
                    new Token(TokenType.Command, "IMUL", new TokenPosition(0, 12)),
                    new Token(TokenType.Command, "IDIV", new TokenPosition(0, 17)),
                    new Token(TokenType.Command, "AND",  new TokenPosition(0, 22)),
                    new Token(TokenType.Command, "OR",   new TokenPosition(0, 26)),
                    new Token(TokenType.Command, "NOT",  new TokenPosition(0, 29)),
                    new Token(TokenType.Command, "XOR",  new TokenPosition(0, 33)),
                    new Token(TokenType.Command, "JMP",  new TokenPosition(0, 37)),
                    new Token(TokenType.Command, "JE",   new TokenPosition(0, 41)),
                    new Token(TokenType.Command, "JNE",  new TokenPosition(0, 44)),
                };

            runTest(sampleString, expectedTokens);
        }

        [TestMethod]
        public void TestRegistersTokenization()
        {
            var sampleString = "AX AH AL " + 
                               "BX BH BL " + 
                               "CX CH CL " + 
                               "DX DH DL " + 
                               "SI DI SP BP";
            
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.Register, "AX", new TokenPosition(0, 0)),
                    new Token(TokenType.Register, "AH", new TokenPosition(0, 3)),
                    new Token(TokenType.Register, "AL", new TokenPosition(0, 6)),

                    new Token(TokenType.Register, "BX", new TokenPosition(0, 9)),
                    new Token(TokenType.Register, "BH", new TokenPosition(0, 12)),
                    new Token(TokenType.Register, "BL", new TokenPosition(0, 15)),
                    
                    new Token(TokenType.Register, "CX", new TokenPosition(0, 18)),
                    new Token(TokenType.Register, "CH", new TokenPosition(0, 21)),
                    new Token(TokenType.Register, "CL", new TokenPosition(0, 24)),
                    
                    new Token(TokenType.Register, "DX", new TokenPosition(0, 27)),
                    new Token(TokenType.Register, "DH", new TokenPosition(0, 30)),
                    new Token(TokenType.Register, "DL", new TokenPosition(0, 33)),

                    new Token(TokenType.Register, "SI", new TokenPosition(0, 36)),
                    new Token(TokenType.Register, "DI", new TokenPosition(0, 39)),
                    new Token(TokenType.Register, "SP", new TokenPosition(0, 42)),
                    new Token(TokenType.Register, "BP", new TokenPosition(0, 45)),
                };

            runTest(sampleString, expectedTokens);
        }

        [TestMethod]
        public void TestIdentifierTokenization()
        {
            var sampleString = "A bxc a_1_2_3 GgGgG10__ movE add1";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.Identifier, "A",         new TokenPosition(0, 0)),
                    new Token(TokenType.Identifier, "BXC",       new TokenPosition(0, 2)),
                    new Token(TokenType.Identifier, "A_1_2_3",   new TokenPosition(0, 6)),
                    new Token(TokenType.Identifier, "GGGGG10__", new TokenPosition(0, 14)),
                    new Token(TokenType.Identifier, "MOVE",      new TokenPosition(0, 24)),
                    new Token(TokenType.Identifier, "ADD1",      new TokenPosition(0, 29))
                };

            runTest(sampleString, expectedTokens);
        }

        [TestMethod]
        public void TestBinConstantTokenization()
        {
            var sampleString = "0110b 0101B";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.BinConstant, "0110B", new TokenPosition(0, 0)),
                    new Token(TokenType.BinConstant, "0101B", new TokenPosition(0, 6))
                };

            runTest(sampleString, expectedTokens);
        }

        [TestMethod]
        public void TestDecConstantTokenization()
        {
            var sampleString = "15d 10D 052";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.DecConstant, "15D", new TokenPosition(0, 0)),
                    new Token(TokenType.DecConstant, "10D", new TokenPosition(0, 4)),
                    new Token(TokenType.DecConstant, "052", new TokenPosition(0, 8))
                };

            runTest(sampleString, expectedTokens);
        }

        [TestMethod]
        public void TestHexConstantTokenization()
        {
            var sampleString = "0a5h 4FfH 012H     ";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.HexConstant, "0A5H", new TokenPosition(0, 0)),
                    new Token(TokenType.HexConstant, "4FFH", new TokenPosition(0, 5)),
                    new Token(TokenType.HexConstant, "012H", new TokenPosition(0, 10))
                };

            runTest(sampleString, expectedTokens);
        }

        [TestMethod]
        public void TestLiteralTokenization()
        {
            var sampleString = @""""" "" Ivan "" ""\""""";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.Literal, @"""""",       new TokenPosition(0, 0)),
                    new Token(TokenType.Literal, @""" Ivan """, new TokenPosition(0, 3)),
                    new Token(TokenType.Literal, @"""\""""",    new TokenPosition(0, 12))
                };

            runTest(sampleString, expectedTokens);
        }

        private List<List<Token>> getTokensLists()
        {
            return new List<List<Token>> 
                {
                    new List<Token> {
                        new Token(TokenType.Identifier, "MESS1"),
                        new Token(TokenType.Directive, "DB"),
                        new Token(TokenType.Literal, @"""Testing: \""print a line:\""""")
                    },
                    new List<Token> {
                        new Token(TokenType.Identifier, "MAXLEN"),
                        new Token(TokenType.Directive, "DB"),
                        new Token(TokenType.DecConstant, "30")
                    },
                    new List<Token> {
                        new Token(TokenType.Directive, "DB"),
                        new Token(TokenType.Literal, @"""$""")
                    },
                    new List<Token> {
                        new Token(TokenType.Identifier, "REAL_LEN"),
                        new Token(TokenType.Directive, "DW"),
                        new Token(TokenType.QuestionMark, "?")
                    },
                    new List<Token> {
                        new Token(TokenType.Identifier, "BEGIN"),
                        new Token(TokenType.Colon, ":"),
                        new Token(TokenType.Command, "JMP"),
                        new Token(TokenType.Identifier, "MAINPROG")
                    },
                    new List<Token> {
                        new Token(TokenType.Command, "MOV"),
                        new Token(TokenType.Register, "AH"),
                        new Token(TokenType.Comma, ","),
                        new Token(TokenType.DecConstant, "09")
                    },
                    new List<Token> {
                        new Token(TokenType.Command, "JE"),
                        new Token(TokenType.Identifier, "EXIT"),
                    },
                    new List<Token> {
                        new Token(TokenType.Command, "MOV"),
                        new Token(TokenType.Register, "AH"),
                        new Token(TokenType.Comma, ","),
                        new Token(TokenType.HexConstant, "40H")
                    },
                    new List<Token> {
                        new Token(TokenType.Command, "MOV"),
                        new Token(TokenType.Register, "BX"),
                        new Token(TokenType.Comma, ","),
                        new Token(TokenType.DecConstant, "1")
                    },
                    new List<Token> {
                        new Token(TokenType.Command, "MOV"),
                        new Token(TokenType.Register, "CL"),
                        new Token(TokenType.Comma, ","),
                        new Token(TokenType.Identifier, "REALLEN")
                    },
                    new List<Token> {
                        new Token(TokenType.Identifier, "EXIT"),
                        new Token(TokenType.Colon, ":"),
                        new Token(TokenType.Command, "ADD"),
                        new Token(TokenType.Register, "AX"),
                        new Token(TokenType.Comma, ","),
                        new Token(TokenType.Register, "DX")
                    },
                    new List<Token> {
                        new Token(TokenType.Command, "SUB"),
                        new Token(TokenType.Register, "CL"),
                        new Token(TokenType.Comma, ","),
                        new Token(TokenType.Register, "AH")
                    },
                    new List<Token> {
                        new Token(TokenType.Command, "IMUL"),
                        new Token(TokenType.BinConstant, "01110B")
                    },
                    new List<Token> {
                        new Token(TokenType.Command, "IDIV"),
                        new Token(TokenType.DecConstant, "846456D")
                    },
                    new List<Token> {
                        new Token(TokenType.Command, "AND"),
                        new Token(TokenType.Register, "SI"),
                        new Token(TokenType.Comma, ","),
                        new Token(TokenType.Register, "DI")
                    },
                    new List<Token> {
                        new Token(TokenType.Command, "XOR"),
                        new Token(TokenType.Register, "BP"),
                        new Token(TokenType.Comma, ","),
                        new Token(TokenType.Register, "SP")
                    },
                    new List<Token> {
                        new Token(TokenType.Command, "JNE"),
                        new Token(TokenType.Identifier, "EXIT")
                    }
                };
        }

        [TestMethod]
        public void TestGeneralTokenization()
        {
            // Arrange.
            var linesOfCode = new List<string>(File.ReadAllLines(Resources.SampleFileForLexer));
            var expectedTokensLists = getTokensLists();

            // Act.
            var actualTokensLists = _testedInstance.Tokenize(linesOfCode);

            // Assert.
            Assert.AreEqual(expectedTokensLists.Count, actualTokensLists.Count());

            int i = 0;
            foreach (var actualTokens in actualTokensLists)
            {
                Assert.AreEqual(
                    expectedTokensLists[i].Count, actualTokens.Count());

                int j = 0;
                foreach (var actualToken in actualTokens)
                {
                    Assert.AreEqual(
                        expectedTokensLists[i][j].Type, actualToken.Type);
                    Assert.AreEqual(
                        expectedTokensLists[i][j].Value, actualToken.Value);

                    ++j;
                }

                ++i;
            }
        }
    }
}
