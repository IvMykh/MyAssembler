using System.Collections.Generic;
using System.Text;
using MyAssembler.Core.LexicalAnalysis;

namespace MyAssembler.UI.ViewModel
{
    class ProtocolItem
    {
        public int      LineNumber  { get; set; }
        public string   AsmCode     { get; set; }
        public string   Address     { get; set; }
        public string   MachineCode { get; set; }

        public ProtocolItem(
            IReadOnlyList<Token> tokens, byte[] bytes, short address)
        {
            LineNumber = tokens[0].Position.Line + 1;
            AsmCode = getAsmCode(tokens);
            Address = string.Format("{0:X2}", address);
            MachineCode = getMachineCode(bytes);

        }

        private string getAsmCode(IReadOnlyList<Token> tokens)
        {
            int i = 0;

            if (tokens[0].Type == TokenType.Identifier &&
                tokens[1].Type == TokenType.Colon)
            {
                i = 2;
            }

            var strBuilder = new StringBuilder();
            strBuilder.Append(tokens[i++].Value);

            while (i < tokens.Count)
            {
                strBuilder.AppendFormat("  {0}", tokens[i++].Value);
            }

            return strBuilder.ToString();
        }
        private string getMachineCode(byte[] bytes)
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendFormat("{0:X2}", bytes[0]);

            for (int i = 1; i < bytes.Length; i++)
            {
                strBuilder.AppendFormat(" {0:X2}", bytes[i]);
            }

            return strBuilder.ToString();
        }
    }
}
