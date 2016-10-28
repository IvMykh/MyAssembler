using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;

namespace MyAssembler.Tests.TranslationTests
{
    [TestClass]
    public class GeneralTranslationTests
    {
        private List<List<Token>> createTokensLists()
        {
            var finalList = new List<List<Token>>();

            finalList.Add(new List<Token> {
                new Token(TokenType.Identifier, "start"),
                new Token(TokenType.Colon, ":"),
                new Token(TokenType.Command, "MOV"),
                new Token(TokenType.Identifier, "fmemcell"),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.BinConstant, "11b")
            });
            finalList.Add(new List<Token> { 
                new Token(TokenType.Command, "MOV"),
                new Token(TokenType.Register, "AX"),
                new Token(TokenType.Comma, ","),
                new Token(TokenType.Register, "BX")
            });
            finalList.Add(new List<Token> { 
                new Token(TokenType.Identifier, "mybyte"),
                new Token(TokenType.Directive, "DB"),
                new Token(TokenType.DecConstant, "5")
            });
            finalList.Add(new List<Token> { 
                new Token(TokenType.Identifier, "myword"),
                new Token(TokenType.Directive, "DW"),
                new Token(TokenType.DecConstant, "50")
            });

            return finalList;
        }

        [TestMethod]
        public void TestIdentifiersCollectionPass()
        {
            // Arrange.
            var tokensLists = createTokensLists();

            var testedTranslationUnits = new TestTranslationUnit[tokensLists.Count];
            for (int i = 0; i < tokensLists.Count; i++)
			{
                testedTranslationUnits[i] = new TestTranslationUnit(tokensLists[i]);
			}

            var context = new TranslationContext(new MyMemoryManager());
            context.AcceptMode = ContextAcceptMode.CollectIdentifiersMode;

            // Act.
            foreach (var item in testedTranslationUnits)
            {
                item.Accept(context);
            }

            // Assert.
            Assert.AreEqual(1, context.MemoryManager.ByteCells.Count);
            Assert.AreEqual(1, context.MemoryManager.WordCells.Count);
            Assert.AreEqual(1, context.MemoryManager.Labels.Count);

            Assert.IsTrue(context.MemoryManager.ByteCells.ContainsKey("mybyte"));
            Assert.IsTrue(context.MemoryManager.WordCells.ContainsKey("myword"));
            Assert.IsTrue(context.MemoryManager.Labels.ContainsKey("start"));
        }
    }
}
