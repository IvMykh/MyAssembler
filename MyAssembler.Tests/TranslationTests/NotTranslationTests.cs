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
    public class NotTranslationTests
        : TranslationTests
    {

        [TestMethod]
        public void TestNotReg0()
        {
            var tokens = new List<Token> { P[PET.NOT], P[PET.CH] };
            var command = new NotCommand(tokens, OperandsSetType.R);

            runTest(command, new List<byte[]> { new byte[] { 0xF6, 0xD5 } });
        }

        [TestMethod]
        public void TestNotReg1()
        {
            var tokens = new List<Token> { P[PET.NOT], P[PET.BX] };
            var command = new NotCommand(tokens, OperandsSetType.R);

            runTest(command, new List<byte[]> { new byte[] { 0xF7, 0xD3 } });
        }

        [TestMethod]
        public void TestNotMem0()
        {
            var tokens = new List<Token> { P[PET.NOT], P[PET.ByteMemCell] };
            var command = new NotCommand(tokens, OperandsSetType.M);

            runTest(command, new List<byte[]> { new byte[] { 0xF6, 0x16, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestNotMem1()
        {
            var tokens = new List<Token> { P[PET.NOT], P[PET.WordMemCell] };
            var command = new NotCommand(tokens, OperandsSetType.M);

            runTest(command, new List<byte[]> { new byte[] { 0xF7, 0x16, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestNotMemLabel()
        {
            var tokens = new List<Token> { P[PET.NOT], P[PET.Label] };
            var command = new NotCommand(tokens, OperandsSetType.M);

            runExpectedExceptionTest(command);
        }
    }
}
