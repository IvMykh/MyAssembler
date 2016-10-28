using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.Translation;
using MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants;

namespace MyAssembler.Tests.ConstantsParsersTests
{
    [TestClass]
    public class BinConstantsParsersTests
    {
        void runBinParse(string bitString, byte[] expectedBytes)
        {
            // Act.
            var testedInstance = new BinConstantsParser();
            byte[] actualBytes = testedInstance.Parse(bitString);

            // Assert.
            Assert.AreEqual(expectedBytes.Length, actualBytes.Length);

            for (int i = 0; i < expectedBytes.Length; i++)
            {
                Assert.AreEqual(expectedBytes[i], actualBytes[i]);
            }
        }


        [TestMethod]
        public void TestBinParserZeroByte()
        {
            // Arrange.
            string bitString = "00000000B";
            byte[] expectedBytes = new byte[] { 0 };

            // Act & assert.
            runBinParse(bitString, expectedBytes);
        }

        [TestMethod]
        public void TestBinParser1NotFullByte()
        {
            // Arrange.
            string bitString = "0000000000110B";
            byte[] expectedBytes = new byte[] { 6 };

            // Act & assert.
            runBinParse(bitString, expectedBytes);
        }

        [TestMethod]
        public void TestBinParser1FullByte()
        {
            // Arrange.
            string bitString = "10100110b";
            byte[] expectedBytes = new byte[] { 166 };

            // Act & assert.
            runBinParse(bitString, expectedBytes);
        }

        [TestMethod]
        public void TestBinParser2FullBytes()
        {
            // Arrange.
            string bitString = "1011010110100110b";
            byte[] expectedBytes = new byte[] { 166, 181 };

            // Act & assert.
            runBinParse(bitString, expectedBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestBinParser3Bytes()
        {
            // Arrange.
            string bitString = "011011010110100110B";
            byte[] expectedBytes = new byte[] { 166, 181, 1 };

            // Act & assert.
            runBinParse(bitString, expectedBytes);
        }
    }
}
