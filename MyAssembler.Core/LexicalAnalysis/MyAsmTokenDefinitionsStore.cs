using System.Collections.Generic;
using System.Text;

namespace MyAssembler.Core.LexicalAnalysis
{
    using CT = CommandType;
    using DT = DirectiveType;
    using RT = RegisterType;

    public enum CommandType
    {
        None,
        MOV,
        ADD,   SUB,   IMUL,   IDIV, 
        AND,   OR,    NOT,    XOR, 
        JMP,   JE,    JNE
    }

    public enum RegisterType
    {
        AX = 1,   AL,   AH, 
        BX,   BL,   BH, 
        CX,   CL,   CH,
        DX,   DL,   DH,
        SI,   DI, 
        SP,   BP
    }

    public enum DirectiveType
    {
        None,
        DB,
        DW 
    }

    public enum SpecialSymbolType
    {
        Comma,
        OpenSquareBracket,
        CloseSquareBracket,
        Colon,
        Plus,
        QuestionMark
    }


    public class MyAsmTokenDefinitionsStore
        : ITokenDefinitionsStore
    {
        /* Special symbols */
        private string _comma;
        private string _openSquareBracket;
        private string _closeSquareBracket;
        private string _colon;
        private string _plus;
        private string _questionMark;
        /* Names */
        private string _directive;
        private string _command;
        private string _identifier;
        private string _register;
        /* Constants */
        private string _binConstant;
        private string _decConstant;
        private string _hexConstant;
        private string _literal;

        
        private static string oneOfMany(params string[] regexes)
        {
            var strBuilder = new StringBuilder();

            if (regexes.Length > 0)
            {
                strBuilder.AppendFormat("^{0}", regexes[0]);
            }

            for (int i = 1; i < regexes.Length; ++i)
            {
                strBuilder.AppendFormat("|^{0}", regexes[i]);
            }

            return strBuilder.ToString();
        }

        public MyAsmTokenDefinitionsStore()
        {
            /* Special symbols */
            _comma                = @"^,";
            _openSquareBracket    = @"^\[";
            _closeSquareBracket   = @"^\]";
            _colon                = @"^:";
            _plus                 = @"^\+";
            _questionMark         = @"^\?";
            /* Names */
            _directive   = oneOfMany(DT.DB.ToString(), 
                                     DT.DW.ToString());

            _command     = oneOfMany(CT.MOV.ToString(), 
                                     CT.ADD.ToString(), CT.SUB.ToString(), CT.IMUL.ToString(), CT.IDIV.ToString(),
                                     CT.AND.ToString(), CT.OR .ToString(), CT.NOT .ToString(), CT.XOR .ToString(),
                                     CT.JMP.ToString(),
                                     CT.JE .ToString(), CT.JNE.ToString());

            _identifier  = @"^[A-Z][_A-Z0-9]*";
            
            _register    = oneOfMany(RT.AX.ToString(), RT.AL.ToString(), RT.AH.ToString(), 
                                     RT.BX.ToString(), RT.BL.ToString(), RT.BH.ToString(), 
                                     RT.CX.ToString(), RT.CL.ToString(), RT.CH.ToString(), 
                                     RT.DX.ToString(), RT.DL.ToString(), RT.DH.ToString(),
                                     RT.SI.ToString(), RT.DI.ToString(), RT.SP.ToString(), RT.BP.ToString());

            /* Constants */
            _binConstant = @"^[01]+B";
            _decConstant = @"^[+-]?[0-9]+D?";
            _hexConstant = @"^[0-9][0-9A-F]*H";
            _literal     = @"^""(?:\\.|[^""\\])*""";
        }

        public List<TokenDefinition> GetTokenDefinitions()
        {
            var tokenDefs = new List<TokenDefinition>()
                {
                    /* Names */
                    new TokenDefinition(TokenType.Directive,          _directive),
                    new TokenDefinition(TokenType.Command,            _command),
                    new TokenDefinition(TokenType.Register,           _register),
                    new TokenDefinition(TokenType.Identifier,         _identifier),
                    /* Constants */
                    new TokenDefinition(TokenType.BinConstant,        _binConstant),
                    new TokenDefinition(TokenType.DecConstant,        _decConstant),
                    new TokenDefinition(TokenType.HexConstant,        _hexConstant),
                    new TokenDefinition(TokenType.Literal,            _literal),
                    /* Special symbols */
                    new TokenDefinition(TokenType.Comma,              _comma),
                    new TokenDefinition(TokenType.OpenSquareBracket,  _openSquareBracket),
                    new TokenDefinition(TokenType.CloseSquareBracket, _closeSquareBracket),
                    new TokenDefinition(TokenType.Plus,               _plus),
                    new TokenDefinition(TokenType.Colon,              _colon),
                    new TokenDefinition(TokenType.QuestionMark,       _questionMark)
                };

            return tokenDefs;
        }
    }
}
