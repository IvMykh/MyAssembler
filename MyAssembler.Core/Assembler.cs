﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.SyntacticAnalysis;
using MyAssembler.Core.Translation.ContextInfrastructure;
using MyAssembler.Core.Translation.TranslationUnits.Abstract;

namespace MyAssembler.Core
{
    public class Assembler
    {
        private List<string> loadFile(string filePath)
        {
            var linesOfCode = new List<string>();

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (var reader = new StreamReader(fileStream, Encoding.Default))
                {
                    while (!reader.EndOfStream)
                    {
                        linesOfCode.Add(reader.ReadLine());
                    }
                }
            }

            return linesOfCode;
        }

        public TranslationResult Translate(string sourceCode)
        {
            var lexer = new Lexer(new MyTokenDefinitionsStore());
            var parser = new Parser(new MyAutomatonBuilder());

            string[] sourceLines = sourceCode.Split('\n');
            List<List<Token>> tokensLists = lexer.Tokenize(sourceLines);
            List<AsmTranslationUnit> translationUnits = parser.Parse(tokensLists);


            var context = new TranslationContext(new MyMemoryManager());
            context.AcceptMode = ContextAcceptMode.CollectIdentifiersMode;

            foreach (var unit in translationUnits)
            {
                unit.Accept(context);
            }

            context.AcceptMode = ContextAcceptMode.TranslateMode;

            foreach (var unit in translationUnits)
            {
                unit.Accept(context);
            }

            context.AcceptMode = ContextAcceptMode.InsertAddressMode;

            foreach (var unit in translationUnits)
            {
                unit.Accept(context);
            }
            
            return new TranslationResult {
                 TokensLists        = tokensLists,
                 TranslatedBytes    = context.TranslatedBytes,
                 Labels             = context.MemoryManager.Labels,
                 ByteCells          = context.MemoryManager.ByteCells,
                 WordCells          = context.MemoryManager.WordCells,
                 Addresses          = context.StartAddresses
            };
        }
    }
}
