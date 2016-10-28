using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.Translation;
using MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants;

namespace MyAssembler.Tests.ConstantsParsersTests
{
    [TestClass]
    public class HexConstantsParsersTests
    {
        void runHexParse(string bitString, byte[] expectedBytes)
        {
            // Act.
            var testedInstance = new HexConstantsParser();
            byte[] actualBytes = testedInstance.Parse(bitString);

            // Assert.
            Assert.AreEqual(expectedBytes.Length, actualBytes.Length);

            for (int i = 0; i < expectedBytes.Length; i++)
            {
                Assert.AreEqual(expectedBytes[i], actualBytes[i]);
            }
        }

        [TestMethod]
        public void TestHexParserZeroByte()
        {
            // Arrange.
            string bitString = "0h";
            byte[] expectedBytes = new byte[] { 0 };

            // Act & assert.
            runHexParse(bitString, expectedBytes);
        }

        [TestMethod]
        public void TestHexParser1Byte()
        {
            // Arrange.
            string bitString = "0ffh";
            byte[] expectedBytes = new byte[] { 255 };

            // Act & assert.
            runHexParse(bitString, expectedBytes);
        }

        [TestMethod]
        public void TestHexParser2Bytes()
        {
            // Arrange.
            string bitString = "1b2EH";
            byte[] expectedBytes = new byte[] { 46, 27 };
        
            // Act & assert.
            runHexParse(bitString, expectedBytes);
        }
        
        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestHexParser3Bytes()
        {
            // Arrange.
            string bitString = "0bffaaH";
            byte[] expectedBytes = new byte[] { 170, 255, 10 };
        
            // Act & assert.
            runHexParse(bitString, expectedBytes);
        }
    }
}
