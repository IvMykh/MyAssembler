using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MyAssembler.Core.LexicalAnalysis;

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
            var sampleString = ",:[]+";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.Comma,              ",", new TokenPosition(0, 0)),
                    new Token(TokenType.Colon,              ":", new TokenPosition(0, 1)),
                    new Token(TokenType.OpenSquareBracket,  "[", new TokenPosition(0, 2)),
                    new Token(TokenType.CloseSquareBracket, "]", new TokenPosition(0, 3)),
                    new Token(TokenType.Plus,               "+", new TokenPosition(0, 4)),
                };

            runTest(sampleString, expectedTokens);
        }

        [TestMethod]
        public void TestDirectivesTokenization()
        {
            var sampleString = "DW db   OrG";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.Directive, "DW",  new TokenPosition(0, 0)),
                    new Token(TokenType.Directive, "db",  new TokenPosition(0, 3)),
                    new Token(TokenType.Directive, "OrG", new TokenPosition(0, 8)),
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
                    new Token(TokenType.Command, "add",  new TokenPosition(0, 4)),
                    new Token(TokenType.Command, "sUb",  new TokenPosition(0, 8)),
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
                    new Token(TokenType.Identifier, "bxc",       new TokenPosition(0, 2)),
                    new Token(TokenType.Identifier, "a_1_2_3",   new TokenPosition(0, 6)),
                    new Token(TokenType.Identifier, "GgGgG10__", new TokenPosition(0, 14)),
                    new Token(TokenType.Identifier, "movE",      new TokenPosition(0, 24)),
                    new Token(TokenType.Identifier, "add1",      new TokenPosition(0, 29))
                };

            runTest(sampleString, expectedTokens);
        }

        [TestMethod]
        public void TestBinConstantTokenization()
        {
            var sampleString = "0110b 0101B";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.BinConstant, "0110b", new TokenPosition(0, 0)),
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
                    new Token(TokenType.DecConstant, "15d", new TokenPosition(0, 0)),
                    new Token(TokenType.DecConstant, "10D", new TokenPosition(0, 4)),
                    new Token(TokenType.DecConstant, "052", new TokenPosition(0, 8))
                };

            runTest(sampleString, expectedTokens);
        }

        [TestMethod]
        public void TestHexConstantTokenization()
        {
            var sampleString = "0a5h 4FfH 012H";
            var expectedTokens = new List<Token> 
                { 
                    new Token(TokenType.HexConstant, "0a5h", new TokenPosition(0, 0)),
                    new Token(TokenType.HexConstant, "4FfH", new TokenPosition(0, 5)),
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



        //[TestMethod]
        //public void TestGeneralTokenization()
        //{
        //    Assert.Fail("Not implemented yet.");
        //}
    }
}
