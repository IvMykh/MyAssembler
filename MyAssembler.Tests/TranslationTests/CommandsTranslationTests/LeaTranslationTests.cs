using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.TranslationUnits.Commands;

namespace MyAssembler.Tests.TranslationTests.CommandsTranslationTests
{
    using MyAssembler.Core.Translation;
    using PET = PoolEntryType;

    [TestClass]
    public class LeaTranslationTests
        : TranslationTests
    {
        [TestMethod]
        public void TestLea0()
        {
            var tokens = new List<Token> { P[PET.LEA], P[PET.DX], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new LeaCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x8D, 0x16, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestLea1()
        {
            var tokens = new List<Token> { P[PET.LEA], P[PET.DX], P[PET.Comma], P[PET.WordMemCell] };
            var command = new LeaCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x8D, 0x16, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestLeaLabel()
        {
            var tokens = new List<Token> { P[PET.LEA], P[PET.DX], P[PET.Comma], P[PET.Label1] };
            var command = new LeaCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }
    }
}
