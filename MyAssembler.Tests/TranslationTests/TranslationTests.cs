using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Tests.TranslationTests
{
    public abstract class TranslationTests
    {
        public TokensPool P { get; protected set;}
        public TranslationContext Context { get; protected set; }

        public TranslationTests()
        {
            P = new TokensPool();

            Mock<IMemoryManager> memoryManagerMock = new Mock<IMemoryManager>();

            memoryManagerMock
                .Setup(m => m.GetIdentifierType(It.Is<string>(s => s == TokensPool.SAMPLE_BYTE_MEMCELL)))
                .Returns(IdentifierType.Byte);

            memoryManagerMock
                .Setup(m => m.GetIdentifierType(It.Is<string>(s => s == TokensPool.SAMPLE_WORD_MEMCELL)))
                .Returns(IdentifierType.Word);

            memoryManagerMock
                .Setup(m => m.GetIdentifierType(It.Is<string>(s => s == TokensPool.SAMPLE_LABEL)))
                .Returns(IdentifierType.Label);

            Context = new TranslationContext(memoryManagerMock.Object);
            Context.AcceptMode = ContextAcceptMode.TranslateMode;
        }

        protected void runTest(AsmTranslationUnit unit, List<byte[]> expectedBytes)
        {
            // Act.
            unit.Accept(Context);

            // Assert.
            assertTest(expectedBytes);
        }
        protected void runExpectedExceptionTest(AsmTranslationUnit unit)
        {
            // Act.
            unit.Accept(Context);

            // Assert.
            Assert.Fail();
        }

        protected void assertTest(List<byte[]> expectedBytes)
        {
            Assert.AreEqual(expectedBytes.Count, Context.TranslatedBytes.Count);

            int i = 0;
            foreach (var actualBytes in Context.TranslatedBytes)
            {
                Assert.AreEqual(expectedBytes[i].Length, actualBytes.Length);
                for (int j = 0; j < expectedBytes[i].Length; j++)
                {
                    Assert.AreEqual(expectedBytes[i][j], actualBytes[j]);
                }

                ++i;
            }
        }
    }
}
