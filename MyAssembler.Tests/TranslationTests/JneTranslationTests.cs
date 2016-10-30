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
    public class JneTranslationTests
        : CommandTranslationTests
    {
        [TestMethod]
        public void TestJneLabel()
        {
            var tokens = new List<Token> { P[PET.JNE], P[PET.Label] };
            var command = new JneCommand(tokens, OperandsSetType.M);

            runTest(command, new List<byte[]> { new byte[] { 0x0F, 0x85, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestJneByteMemCell()
        {
            var tokens = new List<Token> { P[PET.JNE], P[PET.ByteMemCell] };
            var command = new JneCommand(tokens, OperandsSetType.M);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestJneWordMemCell()
        {
            var tokens = new List<Token> { P[PET.JNE], P[PET.WordMemCell] };
            var command = new JneCommand(tokens, OperandsSetType.M);

            runExpectedExceptionTest(command);
        }
    }
}
