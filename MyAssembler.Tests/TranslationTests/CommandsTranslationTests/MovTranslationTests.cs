using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation;
using MyAssembler.Core.Translation.TranslationUnits.Commands;

namespace MyAssembler.Tests.TranslationTests
{
    using PET = PoolEntryType;

    [TestClass]
    public class MovTranslationTests
        : TranslationTests
    {
        // REG-REG
        [TestMethod]
        public void TestMovRegReg0()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.AL], P[PET.Comma], P[PET.BH] };
            var command = new MovCommand(tokens, OperandsSetType.RR);
            
            runTest(command, new List<byte[]> { new byte[] { 0x8A, 0xC7 } });
        }
        
        [TestMethod]
        public void TestMovRegReg1()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CX], P[PET.Comma], P[PET.DX] };
            var command = new MovCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x8B, 0xCA } });
        }
        
        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovRegRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.DH], P[PET.Comma], P[PET.CX] };
            var command = new MovCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovRegRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CX], P[PET.Comma], P[PET.DH] };
            var command = new MovCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }



        // REG-MEM
        [TestMethod]
        public void TestMovRegMem0()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.AH], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new MovCommand(tokens, OperandsSetType.RM);
            
            runTest(command, new List<byte[]> { new byte[] { 0x8A, 0x26, 0x00, 0x00 } });
        }
        
        [TestMethod]
        public void TestMovRegMem1()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.BX], P[PET.Comma], P[PET.WordMemCell] };
            var command = new MovCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x8B, 0x1E, 0x00, 0x00 } });
        }
        
        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovRegMemMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.AH], P[PET.Comma], P[PET.Label1] };
            var command = new MovCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }
        
        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovRegMemMismatch01()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.AH], P[PET.Comma], P[PET.WordMemCell] };
            var command = new MovCommand(tokens, OperandsSetType.RM);
            
            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovRegMemMismatch10()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.AX], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new MovCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }



        // MEM-REG
        [TestMethod]
        public void TestMovMemReg0()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.ByteMemCell], P[PET.Comma], P[PET.BH] };
            var command = new MovCommand(tokens, OperandsSetType.MR);

            runTest(command, new List<byte[]> { new byte[] { 0x88, 0x3E, 0x00, 0x00 } });
        }
        
        [TestMethod]
        public void TestMovMemReg1()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.WordMemCell], P[PET.Comma], P[PET.BX] };
            var command = new MovCommand(tokens, OperandsSetType.MR);

            runTest(command, new List<byte[]> { new byte[] { 0x89, 0x1E, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovMemRegMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.Label1], P[PET.Comma], P[PET.AH] };
            var command = new MovCommand(tokens, OperandsSetType.MR);
            
            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovMemRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.ByteMemCell], P[PET.Comma], P[PET.CX] };
            var command = new MovCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovMemRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.WordMemCell], P[PET.Comma], P[PET.CH] };
            var command = new MovCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }



        // REG-IM
        [TestMethod]
        public void TestMovRegIm00()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CL], P[PET.Comma], P[PET.ByteConst] };
            var command = new MovCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0xB1, 0x64 } });
        }
        
        [TestMethod]
        public void TestMovRegIm10()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CX], P[PET.Comma], P[PET.ByteConst] };
            var command = new MovCommand(tokens, OperandsSetType.RI);
            
            runTest(command, new List<byte[]> { new byte[] { 0xB9, 0x64, 0x00 } });
        }
        
        [TestMethod]
        public void TestMovRegIm11()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CX], P[PET.Comma], P[PET.WordConst] };
            var command = new MovCommand(tokens, OperandsSetType.RI);
            
            runTest(command, new List<byte[]> { new byte[] { 0xB9, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovRegImMismatch()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.CL], P[PET.Comma], P[PET.WordConst] };
            var command = new MovCommand(tokens, OperandsSetType.RI);
            
            runExpectedExceptionTest(command);
        }



        // MEM-IM
        [TestMethod]
        public void TestMovMemIm00()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.ByteMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new MovCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0xC6, 0x06, 0x00, 0x00, 0x64, 0x90 } });
        }

        [TestMethod]
        public void TestMovMemIm10()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.WordMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new MovCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0xC7, 0x06, 0x00, 0x00, 0x64, 0x00 } });
        }
        
        [TestMethod]
        public void TestMovMemIm11()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.WordMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new MovCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0xC7, 0x06, 0x00, 0x00, 0x10, 0x27 } });
        }
        
        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestMovMemImMismatch()
        {
            var tokens = new List<Token> { P[PET.MOV], P[PET.ByteMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new MovCommand(tokens, OperandsSetType.MI);

            runExpectedExceptionTest(command);
        }
    }
}
