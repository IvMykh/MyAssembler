using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;
using MyAssembler.Core.Translation.TranslationUnits.Commands;
using MyAssembler.Core.Translation.TranslationUnits.Directives;

namespace MyAssembler.Tests.TranslationTests
{
    using PET = PoolEntryType;

    [TestClass]
    public class AddressesInsertionTests
        : TranslationTests
    {
        public AddressesInsertionTests()
            : base(0x100, 0x101, 0x105, 0x11A, 0x11C)
        {
        }

        private List<AsmTranslationUnit> createListOfUnits()
        {
            var list = new List<AsmTranslationUnit>();

            // Directives.
            var dbTokens = new List<Token> { P[PET.ByteMemCell], P[PET.DB], P[PET.ByteConst] };
            list.Add(new DbDirective(dbTokens));

            var dwTokens = new List<Token> { P[PET.WordMemCell], P[PET.DW], P[PET.WordConst] };
            list.Add(new DwDirective(dwTokens));

            // Commands
            var movRRTokens = new List<Token> { P[PET.MOV], P[PET.AX], P[PET.Comma], P[PET.BX] };
            list.Add(new MovCommand(movRRTokens, OperandsSetType.RR));

            var addWithLabelTokens =
                new List<Token> { P[PET.Label1], P[PET.Colon], P[PET.ADD], P[PET.AX], P[PET.Comma], P[PET.BX] };
            list.Add(new AddCommand(addWithLabelTokens, OperandsSetType.RR));

            var subRM0Tokens =
                new List<Token> { P[PET.SUB], P[PET.AL], P[PET.Comma], P[PET.ByteMemCell] };
            list.Add(new SubCommand(subRM0Tokens, OperandsSetType.RM));

            var subRM1Tokens =
                new List<Token> { P[PET.SUB], P[PET.AX], P[PET.Comma], P[PET.WordMemCell] };
            list.Add(new SubCommand(subRM1Tokens, OperandsSetType.RM));

            var jmpTokens = new List<Token> { P[PET.JMP], P[PET.Label1] };
            list.Add(new JmpCommand(jmpTokens, OperandsSetType.M));

            var jeTokens = new List<Token> { P[PET.JE], P[PET.Label2] };
            list.Add(new JeCommand(jeTokens, OperandsSetType.M));

            var jneTokens = new List<Token> { P[PET.JNE], P[PET.Label2] };
            list.Add(new JneCommand(jneTokens, OperandsSetType.M));

            var imulTokens = new List<Token> { P[PET.Label2], P[PET.Colon], P[PET.IMUL], P[PET.DX] };
            list.Add(new ImulCommand(imulTokens, OperandsSetType.AR));

            // Additional directive.
            var uninitDbTokens = new List<Token> { 
                P[PET.UninitByteMemCell], P[PET.DB], P[PET.QuestionMark] };
            list.Add(new DbDirective(uninitDbTokens));

            var anotherSubRM0Tokens =
                new List<Token> { P[PET.SUB], P[PET.AL], P[PET.Comma], P[PET.UninitByteMemCell] };
            list.Add(new SubCommand(anotherSubRM0Tokens, OperandsSetType.RM));

            var lea1Tokens = new List<Token> { P[PET.LEA], P[PET.DX], P[PET.Comma], P[PET.WordMemCell] };
            list.Add(new LeaCommand(lea1Tokens, OperandsSetType.RM));

            return list;
        }

        private void checkAddressesInsertion(
            List<byte[]> expectedBytesList, List<short> expectedStartAddressesList)
        {
            Assert.AreEqual(expectedBytesList.Count, Context.TranslatedBytes.Count);
            for (int i = 0; i < expectedBytesList.Count; i++)
            {
                Assert.AreEqual(expectedBytesList[i].Length, Context.TranslatedBytes[i].Length);

                for (int j = 0; j < expectedBytesList[i].Length; j++)
                {
                    Assert.AreEqual(expectedBytesList[i][j], Context.TranslatedBytes[i][j]);
                }
            }

            Assert.AreEqual(expectedStartAddressesList.Count, Context.StartAddresses.Count);
            for (int i = 0; i < expectedStartAddressesList.Count; i++)
            {
                Assert.AreEqual(expectedStartAddressesList[i], Context.StartAddresses[i]);
            }
        }


        [TestMethod]
        public void TestAddressesInsertions()
        {
            // Arrange.
            var list = createListOfUnits();
            var expectedBytesList = new List<byte[]> {
                new byte[] { 0x64 }, 
                new byte[] { 0x10, 0x27 },              // vise versa
                new byte[] { 0x8B, 0xC3 },              // 0x89, 0xD8
                new byte[] { 0x03, 0xC3 },              // 0x01, 0xD8
                new byte[] { 0x2A, 0x06, 0x00, 0x01 },
                new byte[] { 0x2B, 0x06, 0x01, 0x01 },
                new byte[] { 0xE9, 0xF3, 0xFF },
                new byte[] { 0x0F, 0x84, 0x04, 0x00 },
                new byte[] { 0x0F, 0x85, 0x00, 0x00 },
                new byte[] { 0xF7, 0xEA },
                new byte[] { 0x00 },
                new byte[] { 0x2A, 0x06, 0x1C, 0x01 },
                new byte[] { 0x8D, 0x16, 0x01, 0x01 },
            };

            var expectedStartAddressesList = new List<short> {
                0x100, 0x101, 0x103, 0x105, 0x107, 0x10B, 0x10F, 0x112, 0x116, 0x11A, 0x11C, 0x11D, 0x121
            };


            // Act.
            foreach (var asmUnit in list)
            {
                asmUnit.Accept(Context);
            }

            Context.AcceptMode = ContextAcceptMode.InsertAddressMode;
            foreach (var asmUnit in list)
            {
                asmUnit.Accept(Context);
            }

            // Assert.
            checkAddressesInsertion(expectedBytesList, expectedStartAddressesList);
        }
    }
}
