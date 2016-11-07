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
    public class XorTranslationTests
        : TranslationTests
    {
        // REG-REG
        [TestMethod]
        public void TestXorRegReg0()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.DL], P[PET.Comma], P[PET.CH] };
            var command = new XorCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x32, 0xD5 } });
        }

        [TestMethod]
        public void TestXorRegReg1()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.BX], P[PET.Comma], P[PET.AX] };
            var command = new XorCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x33, 0xD8 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestXorRegRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.CL], P[PET.Comma], P[PET.DX] };
            var command = new XorCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestXorRegRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.DX], P[PET.Comma], P[PET.CL] };
            var command = new XorCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }



        // REG-MEM
        [TestMethod]
        public void TestXorRegMem0()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.AH], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new XorCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x32, 0x26, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestXorRegMem1()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.BX], P[PET.Comma], P[PET.WordMemCell] };
            var command = new XorCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x33, 0x1E, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestXorRegMemMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.AH], P[PET.Comma], P[PET.Label1] };
            var command = new XorCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestXorRegMemMismatch01()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.AH], P[PET.Comma], P[PET.WordMemCell] };
            var command = new XorCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestXorRegMemMismatch10()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.AX], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new XorCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }



        // MEM-REG
        [TestMethod]
        public void TestXorMemReg0()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.ByteMemCell], P[PET.Comma], P[PET.DH] };
            var command = new XorCommand(tokens, OperandsSetType.MR);

            runTest(command, new List<byte[]> { new byte[] { 0x30, 0x36, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestXorMemReg1()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.WordMemCell], P[PET.Comma], P[PET.DX] };
            var command = new XorCommand(tokens, OperandsSetType.MR);

            runTest(command, new List<byte[]> { new byte[] { 0x31, 0x16, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestXorMemRegMismatchIdLabel()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.Label1], P[PET.Comma], P[PET.AH] };
            var command = new XorCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestXorMemRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.ByteMemCell], P[PET.Comma], P[PET.CX] };
            var command = new XorCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestXorMemRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.WordMemCell], P[PET.Comma], P[PET.CH] };
            var command = new XorCommand(tokens, OperandsSetType.MR);

            runExpectedExceptionTest(command);
        }



        // REG-IM
        [TestMethod]
        public void TestXorRegIm00()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.CL], P[PET.Comma], P[PET.ByteConst] };
            var command = new XorCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x80, 0xF1, 0x64 } });
        }

        [TestMethod]
        public void TestXorRegIm10()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.CX], P[PET.Comma], P[PET.ByteConst] };
            var command = new XorCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x83, 0xF1, 0x64 } });
        }

        [TestMethod]
        public void TestXorRegIm11()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.DX], P[PET.Comma], P[PET.WordConst] };
            var command = new XorCommand(tokens, OperandsSetType.RI);

            runTest(command, new List<byte[]> { new byte[] { 0x81, 0xF2, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestXorRegImMismatch()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.CL], P[PET.Comma], P[PET.WordConst] };
            var command = new XorCommand(tokens, OperandsSetType.RI);

            runExpectedExceptionTest(command);
        }



        // MEM-IM
        [TestMethod]
        public void TestXorMemIm00()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.ByteMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new XorCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x80, 0x36, 0x00, 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestXorMemIm10()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.WordMemCell], P[PET.Comma], P[PET.ByteConst] };
            var command = new XorCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x83, 0x36, 0x00, 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestXorMemIm11()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.WordMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new XorCommand(tokens, OperandsSetType.MI);

            runTest(command, new List<byte[]> { new byte[] { 0x81, 0x36, 0x00, 0x00, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestXorMemImMismatch()
        {
            var tokens = new List<Token> { P[PET.XOR], P[PET.ByteMemCell], P[PET.Comma], P[PET.WordConst] };
            var command = new XorCommand(tokens, OperandsSetType.MI);

            runExpectedExceptionTest(command);
        }
    }
}
