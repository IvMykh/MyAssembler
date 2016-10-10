using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.Translation;
using MyAssembler.Core.Translation.ParsersForConstants;

namespace MyAssembler.Tests.ConstantsParsersTests
{
    [TestClass]
    public class DecConstantsParsersTests
    {
        void runDecParse(string bitString, byte[] expectedBytes)
        {
            // Act.
            var testedInstance = new DecConstantsParser();
            byte[] actualBytes = testedInstance.Parse(bitString);

            // Assert.
            Assert.AreEqual(expectedBytes.Length, actualBytes.Length);

            for (int i = 0; i < expectedBytes.Length; i++)
            {
                Assert.AreEqual(expectedBytes[i], actualBytes[i]);
            }
        }


        [TestMethod]
        public void TestDecParserZeroByte()
        {
            // Arrange.
            string bitString = "0d";
            byte[] expectedBytes = new byte[] { 0 };

            // Act & assert.
            runDecParse(bitString, expectedBytes);
        }

        [TestMethod]
        public void TestDecParser1Byte()
        {
            // Arrange.
            string bitString = "90d";
            byte[] expectedBytes = new byte[] { 90 };

            // Act & assert.
            runDecParse(bitString, expectedBytes);
        }

        [TestMethod]
        public void TestDecParser2Bytes()
        {
            // Arrange.
            string bitString = "-16513";
            byte[] expectedBytes = new byte[] { 127, 191 };

            // Act & assert.
            runDecParse(bitString, expectedBytes);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestDecParser3Bytes()
        {
            // Arrange.
            string bitString = "70000D";
            byte[] expectedBytes = new byte[] { 112, 23, 1 };
        
            // Act & assert.
            runDecParse(bitString, expectedBytes);
        }
    }
}
