using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Translation;
using MyAssembler.Core.Translation.TranslationUnits.Directives;

namespace MyAssembler.Tests.TranslationTests.DirectivesTranslationTests
{
    using PET = PoolEntryType;

    [TestClass]
    public class DwTranslationTests
        : TranslationTests
    {
        [TestMethod]
        public void TestDwQuestionMark()
        {
            var tokens = new List<Token> { P[PET.ByteMemCell], P[PET.DW], P[PET.QuestionMark] };
            var dtv = new DwDirective(tokens);

            runTest(dtv, new List<byte[]> { new byte[] { 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestDw0()
        {
            var tokens = new List<Token> { P[PET.WordMemCell], P[PET.DW], P[PET.ByteConst] };
            var dtv = new DwDirective(tokens);

            runTest(dtv, new List<byte[]> { new byte[] { 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestDw1()
        {
            var tokens = new List<Token> { P[PET.WordMemCell], P[PET.DW], P[PET.WordConst] };
            var dtv = new DwDirective(tokens);

            runTest(dtv, new List<byte[]> { new byte[] { 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestDwOverflow()
        {
            var tokens = new List<Token> { P[PET.ByteMemCell], P[PET.DB], P[PET.WordOverflow] };
            var dtv = new DbDirective(tokens);

            runExpectedExceptionTest(dtv);
        }
    }
}
