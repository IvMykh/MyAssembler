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
    public class AddTranslationTests
        : TranslationTests
    {
        // REG-REG
        [TestMethod]
        public void TestAddRegReg0()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.DL], P[PET.Comma], P[PET.CH] };
            var command = new AddCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x02, 0xD5 } });
        }

        [TestMethod]
        public void TestAddRegReg1()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.BX], P[PET.Comma], P[PET.AX] };
            var command = new AddCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x03, 0xD8 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAddRegRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.CL], P[PET.Comma], P[PET.DX] };
            var command = new AddCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAddRegRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.DX], P[PET.Comma], P[PET.CL] };
            var command = new AddCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }



        // REG-MEM
        [TestMethod]
        public void TestAddRegMem0()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.AH], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new AddCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x02, 0x26, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestAddRegMem1()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.BX], P[PET.Comma], P[PET.WordMemCell] };
            var command = new AddCommand(tokens, OperandsSetType.RM);
        
            runTest(command, new List<byte[]> { new byte[] { 0x03, 0x1E, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAddRegMemMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.AH], P[PET.Comma], P[PET.Label] };
            var command = new AddCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAddRegMemMismatch01()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.AH], P[PET.Comma], P[PET.WordMemCell] };
            var command = new AddCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAddRegMemMismatch10()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.AX], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new AddCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }



        // MEM-REG
        [TestMethod]
        public void TestAddMemReg0()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.ByteMemCell], P[PET.Comma], P[PET.DH] };
            var command = new AddCommand(tokens, OperandsSetType.MR);

            runTest(command, new List<byte[]> { new byte[] { 0x00, 0x36, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestAddMemReg1()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.WordMemCell], P[PET.Comma], P[PET.DX] };
            var command = new AddCommand(tokens, OperandsSetType.MR);
        
            runTest(command, new List<byte[]> { new byte[] { 0x01, 0x16, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAddMemRegMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.Label], P[PET.Comma], P[PET.AH] };
            var command = new AddCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAddMemRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.ByteMemCell], P[PET.Comma], P[PET.CX] };
            var command = new AddCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAddMemRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.WordMemCell], P[PET.Comma], P[PET.CH] };
            var command = new AddCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }



        // REG-IM
        [TestMethod]
        public void TestAddRegIm00()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.CL], P[PET.Comma], P[PET.ByteConst] };
            var command = new AddCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x80, 0xC1, 0x64 } });
        }

        [TestMethod]
        public void TestAddRegIm10()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.CX], P[PET.Comma], P[PET.ByteConst] };
            var command = new AddCommand(tokens, OperandsSetType.RI);
        
            runTest(command, new List<byte[]> { new byte[] { 0x83, 0xC1, 0x64 } });
        }
        
        [TestMethod]
        public void TestAddRegIm11()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.DX], P[PET.Comma], P[PET.WordConst] };
            var command = new AddCommand(tokens, OperandsSetType.RI);
        
            runTest(command, new List<byte[]> { new byte[] { 0x81, 0xC2, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAddRegImMismatch()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.CL], P[PET.Comma], P[PET.WordConst] };
            var command = new AddCommand(tokens, OperandsSetType.RI);

            runExpectedExceptionTest(command);
        }



        // MEM-IM
        [TestMethod]
        public void TestAddMemIm00()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.ByteMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new AddCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x80, 0x06, 0x00, 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestAddMemIm10()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.WordMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new AddCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x83, 0x06, 0x00, 0x00, 0x64 } });
        }
        
        [TestMethod]
        public void TestAddMemIm11()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.WordMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new AddCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x81, 0x06, 0x00, 0x00, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAddMemImMismatch()
        {
            var tokens = new List<Token> { P[PET.ADD], P[PET.ByteMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new AddCommand(tokens, OperandsSetType.MI);

            runExpectedExceptionTest(command);
        }
    }
}
