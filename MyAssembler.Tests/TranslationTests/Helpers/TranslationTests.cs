using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Tests.TranslationTests
{
    public abstract class TranslationTests
    {
        public static TokensPool P { get; private set; }
        public static Mock<IMemoryManager> CommonMemoryManagerMock { get; private set; }

        static TranslationTests()
        {
            P = new TokensPool();
            CommonMemoryManagerMock = createMemoryManagerMock();
        }
        
        public TranslationContext Context { get; protected set; }

        private static Mock<IMemoryManager> createMemoryManagerMock(
            short byteAddress = 0, 
            short wordAddress = 0, 
            short label1Address = 0, 
            short label2Address = 0,
            short uninitByteAddress = 0)
        {
            Mock<IMemoryManager> memoryManagerMock = new Mock<IMemoryManager>();

            memoryManagerMock
                .Setup(m => m.GetIdentifierType(It.Is<string>(s => s == TokensPool.SAMPLE_BYTE_MEMCELL)))
                .Returns(IdentifierType.Byte);
            memoryManagerMock
                .Setup(m => m.GetIdentifierType(It.Is<string>(s => s == TokensPool.SAMPLE_WORD_MEMCELL)))
                .Returns(IdentifierType.Word);
            memoryManagerMock
                .Setup(m => m.GetIdentifierType(It.Is<string>(s => s == TokensPool.SAMPLE_LABEL_1)))
                .Returns(IdentifierType.Label);
            memoryManagerMock
                .Setup(m => m.GetIdentifierType(It.Is<string>(s => s == TokensPool.SAMPLE_LABEL_2)))
                .Returns(IdentifierType.Label);
            memoryManagerMock
                .Setup(m => m.GetIdentifierType(It.Is<string>(s => s == TokensPool.SAMPLE_UNINIT_BYTE_MEMCELL)))
                .Returns(IdentifierType.Byte);

            var byteCells = new Dictionary<string, short>();
            var wordCells = new Dictionary<string, short>();
            var labels = new Dictionary<string, short>();
            
            memoryManagerMock.SetupGet(m => m.ByteCells).Returns(byteCells);
            memoryManagerMock.SetupGet(m => m.WordCells).Returns(wordCells);
            memoryManagerMock.SetupGet(m => m.Labels).Returns(labels);

            memoryManagerMock
                .Setup(m => m.GetAddressFor(It.Is<string>(s => s == TokensPool.SAMPLE_BYTE_MEMCELL)))
                .Returns(byteAddress);
            memoryManagerMock
                .Setup(m => m.GetAddressFor(It.Is<string>(s => s == TokensPool.SAMPLE_WORD_MEMCELL)))
                .Returns(wordAddress);
            memoryManagerMock
                .Setup(m => m.GetAddressFor(It.Is<string>(s => s == TokensPool.SAMPLE_LABEL_1)))
                .Returns(label1Address);
            memoryManagerMock
                .Setup(m => m.GetAddressFor(It.Is<string>(s => s == TokensPool.SAMPLE_LABEL_2)))
                .Returns(label2Address);
            memoryManagerMock
                .Setup(m => m.GetAddressFor(It.Is<string>(s => s == TokensPool.SAMPLE_UNINIT_BYTE_MEMCELL)))
                .Returns(uninitByteAddress);

            memoryManagerMock
                .Setup(m => m.InsertByteCellAddress(
                    It.Is<string>(s => s == TokensPool.SAMPLE_BYTE_MEMCELL),
                    It.Is<short>(addr => addr == byteAddress)))
                .Callback(() => byteCells[TokensPool.SAMPLE_BYTE_MEMCELL] = byteAddress);

            memoryManagerMock
                .Setup(m => m.InsertWordCellAddress(
                    It.Is<string>(s => s == TokensPool.SAMPLE_WORD_MEMCELL),
                    It.Is<short>(addr => addr == wordAddress)))
                .Callback(() => wordCells[TokensPool.SAMPLE_WORD_MEMCELL] = wordAddress);

            memoryManagerMock
                .Setup(m => m.InsertLabelAddress(
                    It.Is<string>(s => s == TokensPool.SAMPLE_LABEL_1),
                    It.Is<short>(addr => addr == label1Address)))
                .Callback(() => labels[TokensPool.SAMPLE_LABEL_1] = label1Address);

            memoryManagerMock
                .Setup(m => m.InsertLabelAddress(
                    It.Is<string>(s => s == TokensPool.SAMPLE_LABEL_2),
                    It.Is<short>(addr => addr == label2Address)))
                .Callback(() => labels[TokensPool.SAMPLE_LABEL_2] = label2Address);

            memoryManagerMock
                .Setup(m => m.InsertLabelAddress(
                    It.Is<string>(s => s == TokensPool.SAMPLE_UNINIT_BYTE_MEMCELL),
                    It.Is<short>(addr => addr == uninitByteAddress)))
                .Callback(() => labels[TokensPool.SAMPLE_UNINIT_BYTE_MEMCELL] = uninitByteAddress);

            return memoryManagerMock;
        }

        public TranslationTests(
            short byteAddress = 0, 
            short wordAddress = 0, 
            short label1Address = 0, 
            short label2Address = 0,
            short uninitByteAddress = 0)
        {
            Mock<IMemoryManager> memoryManagerMock = null;

            if (byteAddress != 0 ||
                wordAddress != 0 ||
                label1Address != 0 ||
                label2Address != 0 ||
                uninitByteAddress != 0)
            {
                memoryManagerMock = createMemoryManagerMock(
                 byteAddress, wordAddress, label1Address, label2Address, uninitByteAddress);
            }
            else
            {
                memoryManagerMock = CommonMemoryManagerMock;
            }

            Context = new TranslationContext(memoryManagerMock.Object);
            Context.AcceptMode = ContextAcceptMode.TranslateMode;
        }

        protected void runTest(AsmTranslationUnit unit, List<byte[]> expectedBytes)
        {
            // Act.
            unit.Accept(Context);

            // Assert.
            AssertTest(expectedBytes);
        }
        protected void runExpectedExceptionTest(AsmTranslationUnit unit)
        {
            // Act.
            unit.Accept(Context);

            // Assert.
            Assert.Fail();
        }

        protected void AssertTest(List<byte[]> expectedBytes)
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
