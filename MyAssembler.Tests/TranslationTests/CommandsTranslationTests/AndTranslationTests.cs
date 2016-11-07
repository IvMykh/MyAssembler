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
    public class AndTranslationTests
        : TranslationTests
    {
        // REG-REG
        [TestMethod]
        public void TestAndRegReg0()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.DL], P[PET.Comma], P[PET.CH] };
            var command = new AndCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x22, 0xD5 } });
        }

        [TestMethod]
        public void TestAndRegReg1()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.BX], P[PET.Comma], P[PET.AX] };
            var command = new AndCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x23, 0xD8 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAndRegRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.CL], P[PET.Comma], P[PET.DX] };
            var command = new AndCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAndRegRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.DX], P[PET.Comma], P[PET.CL] };
            var command = new AndCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }



        // REG-MEM
        [TestMethod]
        public void TestAndRegMem0()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.AH], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new AndCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x22, 0x26, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestAndRegMem1()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.BX], P[PET.Comma], P[PET.WordMemCell] };
            var command = new AndCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x23, 0x1E, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAndRegMemMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.AH], P[PET.Comma], P[PET.Label1] };
            var command = new AndCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAndRegMemMismatch01()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.AH], P[PET.Comma], P[PET.WordMemCell] };
            var command = new AndCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAndRegMemMismatch10()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.AX], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new AndCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }



        // MEM-REG
        [TestMethod]
        public void TestAndMemReg0()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.ByteMemCell], P[PET.Comma], P[PET.DH] };
            var command = new AndCommand(tokens, OperandsSetType.MR);

            runTest(command, new List<byte[]> { new byte[] { 0x20, 0x36, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestAndMemReg1()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.WordMemCell], P[PET.Comma], P[PET.DX] };
            var command = new AndCommand(tokens, OperandsSetType.MR);

            runTest(command, new List<byte[]> { new byte[] { 0x21, 0x16, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAndMemRegMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.Label1], P[PET.Comma], P[PET.AH] };
            var command = new AndCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAndMemRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.ByteMemCell], P[PET.Comma], P[PET.CX] };
            var command = new AndCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAndMemRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.WordMemCell], P[PET.Comma], P[PET.CH] };
            var command = new AndCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }



        // REG-IM
        [TestMethod]
        public void TestAndRegIm00()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.CL], P[PET.Comma], P[PET.ByteConst] };
            var command = new AndCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x80, 0xE1, 0x64 } });
        }

        [TestMethod]
        public void TestAndRegIm10()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.CX], P[PET.Comma], P[PET.ByteConst] };
            var command = new AndCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x83, 0xE1, 0x64 } });
        }

        [TestMethod]
        public void TestAndRegIm11()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.DX], P[PET.Comma], P[PET.WordConst] };
            var command = new AndCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x81, 0xE2, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAndRegImMismatch()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.CL], P[PET.Comma], P[PET.WordConst] };
            var command = new AndCommand(tokens, OperandsSetType.RI);

            runExpectedExceptionTest(command);
        }



        // MEM-IM
        [TestMethod]
        public void TestAndMemIm00()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.ByteMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new AndCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x80, 0x26, 0x00, 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestAndMemIm10()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.WordMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new AndCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x83, 0x26, 0x00, 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestAndMemIm11()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.WordMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new AndCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x81, 0x26, 0x00, 0x00, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestAndMemImMismatch()
        {
            var tokens = new List<Token> { P[PET.AND], P[PET.ByteMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new AndCommand(tokens, OperandsSetType.MI);

            runExpectedExceptionTest(command);
        }
    }
}
