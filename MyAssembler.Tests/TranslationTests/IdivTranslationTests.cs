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
    public class IdivTranslationTests
        : CommandTranslationTests
    {
        [TestMethod]
        public void TestIdivReg0()
        {
            var tokens = new List<Token> { P[PET.IDIV], P[PET.CH] };
            var command = new IdivCommand(tokens, OperandsSetType.R);

            runTest(command, new List<byte[]> { new byte[] { 0xF6, 0xFD } });
        }

        [TestMethod]
        public void TestIdivReg1()
        {
            var tokens = new List<Token> { P[PET.IDIV], P[PET.BX] };
            var command = new IdivCommand(tokens, OperandsSetType.R);

            runTest(command, new List<byte[]> { new byte[] { 0xF7, 0xFB } });
        }
        
        [TestMethod]
        public void TestIdivMem0()
        {
            var tokens = new List<Token> { P[PET.IDIV], P[PET.ByteMemCell] };
            var command = new IdivCommand(tokens, OperandsSetType.M);

            runTest(command, new List<byte[]> { new byte[] { 0xF6, 0x3E, 0x00, 0x00 } });
        }
        
        [TestMethod]
        public void TestIdivMem1()
        {
            var tokens = new List<Token> { P[PET.IDIV], P[PET.WordMemCell] };
            var command = new IdivCommand(tokens, OperandsSetType.M);

            runTest(command, new List<byte[]> { new byte[] { 0xF7, 0x3E, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestIdivMemLabel()
        {
            var tokens = new List<Token> { P[PET.IDIV], P[PET.Label] };
            var command = new IdivCommand(tokens, OperandsSetType.M);

            runExpectedExceptionTest(command);
        }
    }
}
