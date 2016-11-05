using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Translation;
using MyAssembler.Core.Translation.TranslationUnits.Directives;

namespace MyAssembler.Tests.TranslationTests
{
    using PET = PoolEntryType;

    [TestClass]
    public class DirectivesTranslationTests
        : TranslationTests
    {
        // DB.
        [TestMethod]
        public void TestDbQuestionMark()
        {
            var tokens = new List<Token> { P[PET.ByteMemCell], P[PET.DB], P[PET.QuestionMark] };
            var dtv = new DbDirective(tokens);

            runTest(dtv, new List<byte[]> { new byte[] { 0x00 } });
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

        
        
        // DW.
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
