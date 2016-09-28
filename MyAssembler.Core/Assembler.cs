using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public void Translate(string filePath)
        {
            // ...
        }
    }
}
