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
    public class OrTranslationTests
        : CommandTranslationTests
    {
        // REG-REG
        [TestMethod]
        public void TestOrRegReg0()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.DL], P[PET.Comma], P[PET.CH] };
            var command = new OrCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x0A, 0xD5 } });
        }

        [TestMethod]
        public void TestOrRegReg1()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.BX], P[PET.Comma], P[PET.AX] };
            var command = new OrCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x0B, 0xD8 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestOrRegRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.CL], P[PET.Comma], P[PET.DX] };
            var command = new OrCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestOrRegRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.DX], P[PET.Comma], P[PET.CL] };
            var command = new OrCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }



        // REG-MEM
        [TestMethod]
        public void TestOrRegMem0()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.AH], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new OrCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x0A, 0x26, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestOrRegMem1()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.BX], P[PET.Comma], P[PET.WordMemCell] };
            var command = new OrCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x0B, 0x1E, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestOrRegMemMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.AH], P[PET.Comma], P[PET.Label] };
            var command = new OrCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestOrRegMemMismatch01()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.AH], P[PET.Comma], P[PET.WordMemCell] };
            var command = new OrCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestOrRegMemMismatch10()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.AX], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new OrCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }



        // MEM-REG
        [TestMethod]
        public void TestOrMemReg0()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.ByteMemCell], P[PET.Comma], P[PET.DH] };
            var command = new OrCommand(tokens, OperandsSetType.MR);

            runTest(command, new List<byte[]> { new byte[] { 0x08, 0x36, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestOrMemReg1()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.WordMemCell], P[PET.Comma], P[PET.DX] };
            var command = new OrCommand(tokens, OperandsSetType.MR);

            runTest(command, new List<byte[]> { new byte[] { 0x09, 0x16, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestOrMemRegMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.Label], P[PET.Comma], P[PET.AH] };
            var command = new OrCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestOrMemRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.ByteMemCell], P[PET.Comma], P[PET.CX] };
            var command = new OrCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestOrMemRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.WordMemCell], P[PET.Comma], P[PET.CH] };
            var command = new OrCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }



        // REG-IM
        [TestMethod]
        public void TestOrRegIm00()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.CL], P[PET.Comma], P[PET.ByteConst] };
            var command = new OrCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x80, 0xC9, 0x64 } });
        }

        [TestMethod]
        public void TestOrRegIm10()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.CX], P[PET.Comma], P[PET.ByteConst] };
            var command = new OrCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x83, 0xC9, 0x64 } });
        }

        [TestMethod]
        public void TestOrRegIm11()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.DX], P[PET.Comma], P[PET.WordConst] };
            var command = new OrCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x81, 0xCA, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestOrRegImMismatch()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.CL], P[PET.Comma], P[PET.WordConst] };
            var command = new OrCommand(tokens, OperandsSetType.RI);

            runExpectedExceptionTest(command);
        }



        // MEM-IM
        [TestMethod]
        public void TestOrMemIm00()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.ByteMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new OrCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x80, 0x0E, 0x00, 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestOrMemIm10()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.WordMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new OrCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x83, 0x0E, 0x00, 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestOrMemIm11()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.WordMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new OrCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x81, 0x0E, 0x00, 0x00, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestOrMemImMismatch()
        {
            var tokens = new List<Token> { P[PET.OR], P[PET.ByteMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new OrCommand(tokens, OperandsSetType.MI);

            runExpectedExceptionTest(command);
        }
    }
}
