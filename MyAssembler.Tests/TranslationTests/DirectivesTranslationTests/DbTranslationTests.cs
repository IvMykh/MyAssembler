using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Translation;
using MyAssembler.Core.Translation.TranslationUnits.Directives;

namespace MyAssembler.Tests.TranslationTests.DirectivesTranslationTests
{
    using PET = PoolEntryType;

    [TestClass]
    public class DbTranslationTests
        : TranslationTests
    {
        [TestMethod]
        public void TestDbQuestionMark()
        {
            var tokens = new List<Token> { P[PET.ByteMemCell], P[PET.DB], P[PET.QuestionMark] };
            var dtv = new DbDirective(tokens);

            runTest(dtv, new List<byte[]> { new byte[] { 0x00 } });
        }

        [TestMethod]
        public void TestDbLiteral()
        {
            var tokens = new List<Token> { P[PET.ByteMemCell], P[PET.DB], P[PET.Literal] };
            var dtv = new DbDirective(tokens);

            runTest(dtv, new List<byte[]> { new byte[] { 
                0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x2c, 0x20, 0x77, 0x6f, 0x72, 0x6c, 0x64, 0x21 } 
            });
        }

        [TestMethod]
        public void TestDb0()
        {
            var tokens = new List<Token> { P[PET.ByteMemCell], P[PET.DB], P[PET.ByteConst] };
            var dtv = new DbDirective(tokens);

            runTest(dtv, new List<byte[]> { new byte[] { 0x64 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestDbOverflow()
        {
            var tokens = new List<Token> { P[PET.ByteMemCell], P[PET.DB], P[PET.ByteOverflow] };
            var dtv = new DbDirective(tokens);

            runExpectedExceptionTest(dtv);
        }
    }
}
