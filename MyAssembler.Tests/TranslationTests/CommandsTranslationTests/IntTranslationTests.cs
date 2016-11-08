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
    public class IntTranslationTests
        : TranslationTests
    {
        [TestMethod]
        public void TestInt0()
        {
            var tokens = new List<Token> { P[PET.INT], P[PET.ByteConst] };
            var command = new IntCommand(tokens, OperandsSetType.I);

            runTest(command, new List<byte[]> { new byte[] { 0xCD, 0x64 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestInt1()
        {
            var tokens = new List<Token> { P[PET.INT], P[PET.WordConst] };
            var command = new IntCommand(tokens, OperandsSetType.I);

            runExpectedExceptionTest(command);
        }
    }
}
