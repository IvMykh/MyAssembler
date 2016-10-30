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
    public class JmpTranslationTests
        : CommandTranslationTests
    {
        [TestMethod]
        public void TestJmpLabel()
        {
            var tokens = new List<Token> { P[PET.JMP], P[PET.Label] };
            var command = new JmpCommand(tokens, OperandsSetType.M);

            runTest(command, new List<byte[]> { new byte[] { 0xE9, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestJmpByteMemCell()
        {
            var tokens = new List<Token> { P[PET.JMP], P[PET.ByteMemCell] };
            var command = new JmpCommand(tokens, OperandsSetType.M);

            runExpectedExceptionTest(command);
        }
        
        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestJmpWordMemCell()
        {
            var tokens = new List<Token> { P[PET.JMP], P[PET.WordMemCell] };
            var command = new JmpCommand(tokens, OperandsSetType.M);

            runExpectedExceptionTest(command);
        }
    }
}
