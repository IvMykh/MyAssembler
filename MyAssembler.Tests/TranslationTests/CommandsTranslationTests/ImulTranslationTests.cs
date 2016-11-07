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
    public class ImulTranslationTests
        : TranslationTests
    {
        // AR.
        [TestMethod]
        public void TestImulAxReg0()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.DH] };
            var command = new ImulCommand(tokens, OperandsSetType.AR);

            runTest(command, new List<byte[]> { new byte[] { 0xF6, 0xEE } });
        }

        [TestMethod]
        public void TestImulAxReg1()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.BX] };
            var command = new ImulCommand(tokens, OperandsSetType.AR);

            runTest(command, new List<byte[]> { new byte[] { 0xF7, 0xEB } });
        }



        // AM.
        [TestMethod]
        public void TestImulAxMem0()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.ByteMemCell] };
            var command = new ImulCommand(tokens, OperandsSetType.AM);

            runTest(command, new List<byte[]> { new byte[] { 0xF6, 0x2E, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestImulAxMem1()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.WordMemCell] };
            var command = new ImulCommand(tokens, OperandsSetType.AM);

            runTest(command, new List<byte[]> { new byte[] { 0xF7, 0x2E, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulAxMemMismatch()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.Label1] };
            var command = new ImulCommand(tokens, OperandsSetType.AM);

            runExpectedExceptionTest(command);
        }



        // RR.
        [TestMethod]
        public void TestImulRegReg0()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.BH], P[PET.Comma], P[PET.CL] };
            var command = new ImulCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x0F, 0xAF, 0xF9 } });
        }

        [TestMethod]
        public void TestImulRegReg1()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.CX] };
            var command = new ImulCommand(tokens, OperandsSetType.RR);

            runTest(command, new List<byte[]> { new byte[] { 0x0F, 0xAF, 0xD9 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegRegMismatch01()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.BL], P[PET.Comma], P[PET.CX] };
            var command = new ImulCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegRegMismatch10()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.CL] };
            var command = new ImulCommand(tokens, OperandsSetType.RR);

            runExpectedExceptionTest(command);
        }



        // RM.
        [TestMethod]
        public void TestImulRegMem0()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.BH], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new ImulCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x0F, 0xAF, 0x3E, 0x00, 0x00 } });
        }

        [TestMethod]
        public void TestImulRegMem1()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.WordMemCell] };
            var command = new ImulCommand(tokens, OperandsSetType.RM);

            runTest(command, new List<byte[]> { new byte[] { 0x0F, 0xAF, 0x1E, 0x00, 0x00 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegMemMismatchLabel()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.Label1] };
            var command = new ImulCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegMemMismatch01()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.CL], P[PET.Comma], P[PET.WordMemCell] };
            var command = new ImulCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegMemMismatch10()
        {
            var tokens = new List<Token> { P[PET.IMUL], P[PET.CX], P[PET.Comma], P[PET.ByteMemCell] };
            var command = new ImulCommand(tokens, OperandsSetType.RM);

            runExpectedExceptionTest(command);
        }



        // RRI.
        [TestMethod]
        public void TestImulRegRegIm000()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BH], P[PET.Comma], P[PET.CL], P[PET.Comma], P[PET.ByteConst] 
            };
            var command = new ImulCommand(tokens, OperandsSetType.RRI);

            runTest(command, new List<byte[]> { new byte[] { 0x69, 0xF9, 0x64 } });
        }

        [TestMethod]
        public void TestImulRegRegIm110()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.CX], P[PET.Comma], P[PET.ByteConst] 
            };
            var command = new ImulCommand(tokens, OperandsSetType.RRI);

            runTest(command, new List<byte[]> { new byte[] { 0x6B, 0xD9, 0x64 } });
        }

        [TestMethod]
        public void TestImulRegRegIm111()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.CX], P[PET.Comma], P[PET.WordConst] 
            };
            var command = new ImulCommand(tokens, OperandsSetType.RRI);

            runTest(command, new List<byte[]> { new byte[] { 0x69, 0xD9, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegRegImMismatch010()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BL], P[PET.Comma], P[PET.CX], P[PET.Comma], P[PET.ByteConst]
            };
            var command = new ImulCommand(tokens, OperandsSetType.RRI);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegRegImMismatch100()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.CL], P[PET.Comma], P[PET.ByteConst]
            };
            var command = new ImulCommand(tokens, OperandsSetType.RRI);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegRegImMismatch001()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BL], P[PET.Comma], P[PET.CL], P[PET.Comma], P[PET.WordConst]
            };
            var command = new ImulCommand(tokens, OperandsSetType.RRI);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegRegImMismatch011()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BL], P[PET.Comma], P[PET.CX], P[PET.Comma], P[PET.WordConst]
            };
            var command = new ImulCommand(tokens, OperandsSetType.RRI);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegRegImMismatch101()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.CL], P[PET.Comma], P[PET.WordConst]
            };
            var command = new ImulCommand(tokens, OperandsSetType.RRI);

            runExpectedExceptionTest(command);
        }



        // RMI.
        [TestMethod]
        public void TestImulRegMemIm000()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BH], P[PET.Comma], P[PET.ByteMemCell], P[PET.Comma], P[PET.ByteConst] 
            };
            var command = new ImulCommand(tokens, OperandsSetType.RMI);
        
            runTest(command, new List<byte[]> { new byte[] { 0x69, 0x3E, 0x00, 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestImulRegMemIm110()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.WordMemCell], P[PET.Comma], P[PET.ByteConst] 
            };
            var command = new ImulCommand(tokens, OperandsSetType.RMI);

            runTest(command, new List<byte[]> { new byte[] { 0x6B, 0x1E, 0x00, 0x00, 0x64 } });
        }

        [TestMethod]
        public void TestImulRegMemIm111()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.WordMemCell], P[PET.Comma], P[PET.WordConst] 
            };
            var command = new ImulCommand(tokens, OperandsSetType.RMI);

            runTest(command, new List<byte[]> { new byte[] { 0x69, 0x1E, 0x00, 0x00, 0x10, 0x27 } });
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegMemImMismatch010()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BL], P[PET.Comma], P[PET.WordMemCell], P[PET.Comma], P[PET.ByteConst]
            };
            var command = new ImulCommand(tokens, OperandsSetType.RMI);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegMemImMismatch100()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.ByteMemCell], P[PET.Comma], P[PET.ByteConst]
            };
            var command = new ImulCommand(tokens, OperandsSetType.RMI);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegMemImMismatch101()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BX], P[PET.Comma], P[PET.ByteMemCell], P[PET.Comma], P[PET.WordConst]
            };
            var command = new ImulCommand(tokens, OperandsSetType.RMI);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegMemImMismatch011()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BL], P[PET.Comma], P[PET.WordMemCell], P[PET.Comma], P[PET.WordConst]
            };
            var command = new ImulCommand(tokens, OperandsSetType.RMI);

            runExpectedExceptionTest(command);
        }

        [TestMethod]
        [ExpectedException(typeof(TranslationErrorException))]
        public void TestImulRegMemImMismatch001()
        {
            var tokens = new List<Token> { 
                P[PET.IMUL], P[PET.BL], P[PET.Comma], P[PET.ByteMemCell], P[PET.Comma], P[PET.WordConst]
            };
            var command = new ImulCommand(tokens, OperandsSetType.RMI);

            runExpectedExceptionTest(command);
        }
    }
}
