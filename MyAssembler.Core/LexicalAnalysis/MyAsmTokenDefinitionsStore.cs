using System.Collections.Generic;
using System.Text;

namespace MyAssembler.Core.LexicalAnalysis
{
    public class MyAsmTokenDefinitionsStore
        : ITokenDefinitionsStore
    {
        /* Special symbols */
        public string Comma              { get; private set; }
        public string OpenSquareBracket  { get; private set; }
        public string CloseSquareBracket { get; private set; }
        public string Colon              { get; private set; }
        public string Plus               { get; private set; }
        /* Names */
        public string Directive          { get; private set; }
        public string Command            { get; private set; }
        public string Identifier         { get; private set; }
        public string Register           { get; private set; }
        /* Constants */
        public string BinConstant        { get; private set; }
        public string DecConstant        { get; private set; }
        public string HexConstant        { get; private set; }
        public string Literal            { get; private set; }

        
        private static string oneOfMany(params string[] regexes)
        {
            var strBuilder = new StringBuilder();

            if (regexes.Length > 0)
            {
                strBuilder.Append(regexes[0]);
            }

            for (int i = 1; i < regexes.Length; ++i)
            {
                strBuilder.AppendFormat("|{0}", regexes[i]);
            }

            return strBuilder.ToString();
        }

        public MyAsmTokenDefinitionsStore()
        {
            /* Special symbols */
            Comma                = @"^,";
            OpenSquareBracket    = @"^\[";
            CloseSquareBracket   = @"^\]";
            Colon                = @"^:";
            Plus                 = @"^\+";
            /* Names */
            Directive   = oneOfMany("^DW", "^DB", "^ORG");
            Command     = oneOfMany("^MOV", 
                                    "^ADD", "^SUB", "^IMUL", "^IDIV",
                                    "^AND", "^OR", "^NOT", "^XOR",
                                    "^JMP",
                                    "^JE", "^JNE");
            Identifier  = @"^[A-Z][_A-Z0-9]*";
            Register    = oneOfMany("^(AX|AL|AH)", "^(BX|BL|BH)", "^(CX|CL|CH)", "^(DX|DL|DH)",
                                    "^SI", "^DI", "^SP", "^BP");
            /* Constants */
            BinConstant = @"^[01]+B";
            DecConstant = @"^[+-]?[0-9]+D?";
            HexConstant = @"^[0-9][0-9A-F]*H";
            Literal     = @"^""(?:\\.|[^""\\])*""";
        }

        public IEnumerable<TokenDefinition> GetTokenDefinitions()
        {
            var tokenDefs = new List<TokenDefinition>()
                {
                    /* Names */
                    new TokenDefinition(TokenType.Directive,  Directive),
                    new TokenDefinition(TokenType.Command,    Command),
                    new TokenDefinition(TokenType.Register,   Register),
                    new TokenDefinition(TokenType.Identifier, Identifier),
                    /* Constants */
                    new TokenDefinition(TokenType.BinConstant, BinConstant),
                    new TokenDefinition(TokenType.DecConstant, DecConstant),
                    new TokenDefinition(TokenType.HexConstant, HexConstant),
                    new TokenDefinition(TokenType.Literal,     Literal),
                    /* Special symbols */
                    new TokenDefinition(TokenType.Comma,              Comma),
                    new TokenDefinition(TokenType.OpenSquareBracket,  OpenSquareBracket),
                    new TokenDefinition(TokenType.CloseSquareBracket, CloseSquareBracket),
                    new TokenDefinition(TokenType.Plus,               Plus),
                    new TokenDefinition(TokenType.Colon,              Colon)
                };

            return tokenDefs;
        }
    }
}
