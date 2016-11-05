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
    public class SubTranslationTests
        : TranslationTests
    {
        // REG-REG
        [TestMethod]
        public void TestSubRegReg0()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.DL], P[PET.Comma], P[PET.CH] };
            var command = new SubCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x2A, 0xD5 } });
        }

        [TestMethod]
        public void TestSubRegReg1()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.BX], P[PET.Comma], P[PET.AX] };
            var command = new SubCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x2B, 0xD8 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestSubRegRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.CL], P[PET.Comma], P[PET.DX] };
            var command = new SubCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestSubRegRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.DX], P[PET.Comma], P[PET.CL] };
            var command = new SubCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }



        // REG-MEM
        [TestMethod]
        public void TestSubRegMem0()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.AH], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new SubCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x2A, 0x26, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestSubRegMem1()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.BX], P[PET.Comma], P[PET.WordMemCell] };
            var command = new SubCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x2B, 0x1E, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestSubRegMemMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.AH], P[PET.Comma], P[PET.Label] };
            var command = new SubCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestSubRegMemMismatch01()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.AH], P[PET.Comma], P[PET.WordMemCell] };
            var command = new SubCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestSubRegMemMismatch10()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.AX], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new SubCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }



        // MEM-REG
        [TestMethod]
        public void TestSubMemReg0()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.ByteMemCell], P[PET.Comma], P[PET.DH] };
            var command = new SubCommand(tokens, OperandsSetType.MR);

            runTest(command, new List<byte[]> { new byte[] { 0x28, 0x36, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestSubMemReg1()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.WordMemCell], P[PET.Comma], P[PET.DX] };
            var command = new SubCommand(tokens, OperandsSetType.MR);

            runTest(command, new List<byte[]> { new byte[] { 0x29, 0x16, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestSubMemRegMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.Label], P[PET.Comma], P[PET.AH] };
            var command = new SubCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestSubMemRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.ByteMemCell], P[PET.Comma], P[PET.CX] };
            var command = new SubCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestSubMemRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.WordMemCell], P[PET.Comma], P[PET.CH] };
            var command = new SubCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }



        // REG-IM
        [TestMethod]
        public void TestSubRegIm00()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.CL], P[PET.Comma], P[PET.ByteConst] };
            var command = new SubCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x80, 0xE9, 0x64 } });
        }

        [TestMethod]
        public void TestSubRegIm10()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.CX], P[PET.Comma], P[PET.ByteConst] };
            var command = new SubCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x83, 0xE9, 0x64 } });
        }

        [TestMethod]
        public void TestSubRegIm11()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.DX], P[PET.Comma], P[PET.WordConst] };
            var command = new SubCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x81, 0xEA, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestSubRegImMismatch()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.CL], P[PET.Comma], P[PET.WordConst] };
            var command = new SubCommand(tokens, OperandsSetType.RI);

            runExpectedExceptionTest(command);
        }



        // MEM-IM
        [TestMethod]
        public void TestSubMemIm00()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.ByteMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new SubCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x80, 0x2E, 0x00, 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestSubMemIm10()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.WordMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new SubCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x83, 0x2E, 0x00, 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestSubMemIm11()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.WordMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new SubCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x81, 0x2E, 0x00, 0x00, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestSubMemImMismatch()
        {
            var tokens = new List<Token> { P[PET.SUB], P[PET.ByteMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new SubCommand(tokens, OperandsSetType.MI);

            runExpectedExceptionTest(command);
        }
    }
}
