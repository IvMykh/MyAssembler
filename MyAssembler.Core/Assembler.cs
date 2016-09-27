using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAssembler.Core.Properties;

namespace MyAssembler.Core
{
    public class Assembler
    {
        private IEnumerable<string> loadFile(string filePath)
        {
            string[] linesOfCode = File.ReadAllLines(filePath, Encoding.Default);
            prepareCode(linesOfCode);

            return linesOfCode;
        }

        private void prepareCode(string[] lines)
        {
            var commentStartSymbol = char.Parse(Resources.CommentStartSymbol);

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = 
                    lines[i].Remove(lines[i].IndexOf(commentStartSymbol))
                            .Trim();
            }
        }

        public void Translate(string filePath)
        {
            // ...
        }
    }
}
