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
    public class JeTranslationTests
        : CommandTranslationTests
    {
        [TestMethod]
        public void TestJeLabel()
        {
            var tokens = new List<Token> { P[PET.JE], P[PET.Label] };
            var command = new JeCommand(tokens, OperandsSetType.M);

            runTest(command, new List<byte[]> { new byte[] { 0x0F, 0x84, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestJeByteMemCell()
        {
            var tokens = new List<Token> { P[PET.JE], P[PET.ByteMemCell] };
            var command = new JeCommand(tokens, OperandsSetType.M);
            
            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestJeWordMemCell()
        {
            var tokens = new List<Token> { P[PET.JE], P[PET.WordMemCell] };
            var command = new JeCommand(tokens, OperandsSetType.M);

            runExpectedExceptionTest(command);
        }
    }
}
