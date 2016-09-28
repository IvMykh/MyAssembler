using System.Collections.Generic;
using System.IO;
using System.Text;

using MyAssembler.Core.Properties;

namespace MyAssembler.Core
{
    public class Assembler
    {
        private string[] loadFile(string filePath)
        {
            string[] linesOfCode = File.ReadAllLines(filePath, Encoding.Default);
            return linesOfCode;
        }

        public void Translate(string filePath)
        {
            // ...
        }
    }
}
