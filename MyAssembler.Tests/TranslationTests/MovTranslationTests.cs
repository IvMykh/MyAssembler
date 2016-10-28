using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.TranslationUnits.Commands;

namespace MyAssembler.Tests.TranslationTests
{
    using PET = PoolEntryType;

    [TestClass]
    public class MovTranslationTests
    {
        private TokensPool         P;
        private TranslationContext _context;

        public MovTranslationTests()
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

            _context = new TranslationContext(memoryManagerMock.Object);
            _context.AcceptMode = ContextAcceptMode.TranslateMode;
        }

        private MovCommand createRegReg0()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.AL], P[PET.Comma], P[PET.BH] };
            return new MovCommand(tokens, OperandsSetType.RR);
        }
        private MovCommand createRegReg1()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CX], P[PET.Comma], P[PET.DX] };
            return new MovCommand(tokens, OperandsSetType.RR);
        }
        private MovCommand createRegRegMismatch()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CX], P[PET.Comma], P[PET.DH] };
            return new MovCommand(tokens, OperandsSetType.RR);
        }

        private MovCommand createRegMem0()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.AH], P[PET.Comma], P[PET.ByteMemCell] };
            return new MovCommand(tokens, OperandsSetType.RM);
        }
        private MovCommand createRegMem1()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.BX], P[PET.Comma], P[PET.WordMemCell] };
            return new MovCommand(tokens, OperandsSetType.RM);
        }
        private MovCommand createRegMemMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.AH], P[PET.Comma], P[PET.Label] };
            return new MovCommand(tokens, OperandsSetType.RM);
        }
        private MovCommand createRegMemMismatch01()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.AH], P[PET.Comma], P[PET.WordMemCell] };
            return new MovCommand(tokens, OperandsSetType.RM);
        }
        private MovCommand createRegMemMismatch10()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.AX], P[PET.Comma], P[PET.ByteMemCell] };
            return new MovCommand(tokens, OperandsSetType.RM);
        }

        private MovCommand createMemReg0()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.ByteMemCell], P[PET.Comma], P[PET.BH] };
            return new MovCommand(tokens, OperandsSetType.MR);
        }
        private MovCommand createMemReg1()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.WordMemCell], P[PET.Comma], P[PET.BX] };
            return new MovCommand(tokens, OperandsSetType.MR);
        }
        private MovCommand createMemRegMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.Label], P[PET.Comma], P[PET.AH] };
            return new MovCommand(tokens, OperandsSetType.MR);
        }
        private MovCommand createMemRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.ByteMemCell], P[PET.Comma], P[PET.CX] };
            return new MovCommand(tokens, OperandsSetType.MR);
        }
        private MovCommand createMemRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.WordMemCell], P[PET.Comma], P[PET.CH] };
            return new MovCommand(tokens, OperandsSetType.MR);
        }

        private MovCommand createRegIm00()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CL], P[PET.Comma], P[PET.ByteConst] };
            return new MovCommand(tokens, OperandsSetType.RI);
        }
        private MovCommand createRegIm10()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CX], P[PET.Comma], P[PET.ByteConst] };
            return new MovCommand(tokens, OperandsSetType.RI);
        }
        private MovCommand createRegIm11()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CX], P[PET.Comma], P[PET.WordConst] };
            return new MovCommand(tokens, OperandsSetType.RI);
        }
        private MovCommand createRegImMismatch()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CL], P[PET.Comma], P[PET.WordConst] };
            return new MovCommand(tokens, OperandsSetType.RI);
        }

        private MovCommand createMemIm00()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.ByteMemCell], P[PET.Comma], P[PET.ByteConst] };
            return new MovCommand(tokens, OperandsSetType.MI);
        }
        private MovCommand createMemIm10()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.WordMemCell], P[PET.Comma], P[PET.ByteConst] };
            return new MovCommand(tokens, OperandsSetType.MI);
        }
        private MovCommand createMemIm11()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.WordMemCell], P[PET.Comma], P[PET.WordConst] };
            return new MovCommand(tokens, OperandsSetType.MI);
        }
        private MovCommand createMemImMismatch()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.ByteMemCell], P[PET.Comma], P[PET.WordConst] };
            return new MovCommand(tokens, OperandsSetType.MI);
        }


        private void runTest(Func<MovCommand> initFunc, List<byte[]> expectedBytes)
        {
            // Arrange.
            MovCommand mov = initFunc();

            // Act.
            mov.Accept(_context);

            // Assert.
            assertTest(expectedBytes);
        }
        private void runExpectedExceptionTest(Func<MovCommand> initFunc)
        {
            // Arrange.
            MovCommand mov = initFunc();

            // Act.
            mov.Accept(_context);

            // Assert.
            Assert.Fail();
        }

        private void assertTest(List<byte[]> expectedBytes)
        {
            Assert.AreEqual(expectedBytes.Count, _context.TranslatedBytes.Count);

            int i = 0;
            foreach (var actualBytes in _context.TranslatedBytes)
            {
                Assert.AreEqual(expectedBytes[i].Length, actualBytes.Length);
                for (int j = 0; j < expectedBytes[i].Length; j++)
                {
                    Assert.AreEqual(expectedBytes[i][j], actualBytes[j]);
                }

                ++i;
            }
        }


        [TestMethod]
        public void TestMovRegReg0()
        {
            runTest(createRegReg0, new List<byte[]> { new byte[] { 0x8A, 0xC7 } });
        }
        
        [TestMethod]
        public void TestMovRegReg1()
        {
            runTest(createRegReg1, new List<byte[]> { new byte[] { 0x8B, 0xCA } });
        }
        
        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovRegRegMismatch()
        {
            runExpectedExceptionTest(createRegRegMismatch);
        }



        [TestMethod]
        public void TestMovRegMem0()
        {
            runTest(createRegMem0, new List<byte[]> { new byte[] { 0x8A, 0x26, 0x00, 0x00 } });
        }
        
        [TestMethod]
        public void TestMovRegMem1()
        {
            runTest(createRegMem1, new List<byte[]> { new byte[] { 0x8B, 0x1E, 0x00, 0x00 } });
        }
        
        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovRegMemMismatchIdLabel()
        {
            runExpectedExceptionTest(createRegMemMismatchIdLabel);
        }
        
        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovRegMemMismatch01()
        {
            runExpectedExceptionTest(createRegMemMismatch01);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovRegMemMismatch10()
        {
            runExpectedExceptionTest(createRegMemMismatch10);
        }



        [TestMethod]
        public void TestMovMemReg0()
        {
            runTest(createMemReg0, new List<byte[]> { new byte[] { 0x88, 0x3E, 0x00, 0x00 } });
        }
        
        [TestMethod]
        public void TestMovMemReg1()
        {
            runTest(createMemReg1, new List<byte[]> { new byte[] { 0x89, 0x1E, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovMemRegMismatchIdLabel()
        {
            runExpectedExceptionTest(createMemRegMismatchIdLabel);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovMemRegMismatch01()
        {
            runExpectedExceptionTest(createMemRegMismatch01);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovMemRegMismatch10()
        {
            runExpectedExceptionTest(createMemRegMismatch10);
        }



        [TestMethod]
        public void TestMovRegIm00()
        {
            runTest(createRegIm00, new List<byte[]> { new byte[] { 0xB1, 0x64 } });
        }
        
        [TestMethod]
        public void TestMovRegIm10()
        {
            runTest(createRegIm10, new List<byte[]> { new byte[] { 0xB9, 0x64, 0x00 } });
        }
        
        [TestMethod]
        public void TestMovRegIm11()
        {
            runTest(createRegIm11, new List<byte[]> { new byte[] { 0xB9, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovRegImMismatch()
        {
            runExpectedExceptionTest(createRegImMismatch);
        }



        [TestMethod]
        public void TestMovMemIm00()
        {
            runTest(createMemIm00, new List<byte[]> { new byte[] { 0xC6, 0x06, 0x00, 0x00, 0x64, 0x90 } });
        }

        [TestMethod]
        public void TestMovMemIm10()
        {
            runTest(createMemIm10, new List<byte[]> { new byte[] { 0xC7, 0x06, 0x00, 0x00, 0x64, 0x00 } });
        }
        
        [TestMethod]
        public void TestMovMemIm11()
        {
            runTest(createMemIm11, new List<byte[]> { new byte[] { 0xC7, 0x06, 0x00, 0x00, 0x10, 0x27 } });
        }
        
        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovMemImMismatch()
        {
            runExpectedExceptionTest(createMemImMismatch);
        }
    }
}
