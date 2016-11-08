using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.Translation.ContextInfrastructure.ParsersForConstants;

namespace MyAssembler.Tests.ConstantsParsersTests
{
    [TestClass]
    public class LiteralsParsersTests
    {
        [TestMethod]
        public void TestLiteralParser()
        {
            // Arrange.
            string sampleLiteral = "Hello, world!";
            var testedInstance = new LiteralParser();
            var expectedBytes = new byte[] {
                0x48, 0x65, 0x6c, 0x6c, 0x6f, 0x2c, 0x20, 0x77, 0x6f, 0x72, 0x6c, 0x64, 0x21 
            };

            // Act.
            byte[] parsedBytes = testedInstance.Parse(sampleLiteral);

            // Assert.
            Assert.AreEqual(expectedBytes.Length, parsedBytes.Length);

            for (int i = 0; i < expectedBytes.Length; i++)
            {
                Assert.AreEqual(expectedBytes[i], parsedBytes[i]);
            }
        }
    }
}
