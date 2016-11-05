using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;

namespace MyAssembler.Tests.TranslationTests
{
    public enum PoolEntryType
    {
        MOV,
        ADD, SUB,
        AND, OR, XOR, NOT,
        JE, JNE, JMP,
        IMUL, IDIV,
        Comma,
        AH, AL, AX,
        BH, BL, BX,
        CH, CL, CX,
        DH, DL, DX,
        ByteMemCell,
        WordMemCell,
        Label,
        ByteConst,
        WordConst
    }

    public class TokensPool
    {
        public const string SAMPLE_BYTE_MEMCELL = "ByteMemcell";
        public const string SAMPLE_WORD_MEMCELL = "WordMemcell";
        public const string SAMPLE_LABEL        = "label";
        public const string SAMPLE_BYTE_CONST   = "100";
        public const string SAMPLE_WORD_CONST   = "10000";

        Dictionary<PoolEntryType, Token> _pool;

        public TokensPool()
        {
            _pool = new Dictionary<PoolEntryType, Token>();

            _pool.Add(PoolEntryType.MOV, new Token(TokenType.Command, "MOV"));
            _pool.Add(PoolEntryType.ADD, new Token(TokenType.Command, "ADD"));
            _pool.Add(PoolEntryType.SUB, new Token(TokenType.Command, "SUB"));
            _pool.Add(PoolEntryType.AND, new Token(TokenType.Command, "AND"));
            _pool.Add(PoolEntryType.OR, new Token(TokenType.Command, "OR"));
            _pool.Add(PoolEntryType.XOR, new Token(TokenType.Command, "XOR"));
            _pool.Add(PoolEntryType.NOT, new Token(TokenType.Command, "NOT"));
            _pool.Add(PoolEntryType.JE, new Token(TokenType.Command, "JE"));
            _pool.Add(PoolEntryType.JNE, new Token(TokenType.Command, "JNE"));
            _pool.Add(PoolEntryType.JMP, new Token(TokenType.Command, "JMP"));
            _pool.Add(PoolEntryType.IMUL, new Token(TokenType.Command, "IMUL"));
            _pool.Add(PoolEntryType.IDIV, new Token(TokenType.Command, "IDIV"));

            _pool.Add(PoolEntryType.Comma, new Token(TokenType.Comma, ","));
            
            _pool.Add(PoolEntryType.AH, new Token(TokenType.Register, RegisterType.AH.ToString()));
            _pool.Add(PoolEntryType.AL, new Token(TokenType.Register, RegisterType.AL.ToString()));
            _pool.Add(PoolEntryType.AX, new Token(TokenType.Register, RegisterType.AX.ToString()));
            _pool.Add(PoolEntryType.BH, new Token(TokenType.Register, RegisterType.BH.ToString()));
            _pool.Add(PoolEntryType.BL, new Token(TokenType.Register, RegisterType.BL.ToString()));
            _pool.Add(PoolEntryType.BX, new Token(TokenType.Register, RegisterType.BX.ToString()));
            _pool.Add(PoolEntryType.CH, new Token(TokenType.Register, RegisterType.CH.ToString()));
            _pool.Add(PoolEntryType.CL, new Token(TokenType.Register, RegisterType.CL.ToString()));
            _pool.Add(PoolEntryType.CX, new Token(TokenType.Register, RegisterType.CX.ToString()));
            _pool.Add(PoolEntryType.DH, new Token(TokenType.Register, RegisterType.DH.ToString()));
            _pool.Add(PoolEntryType.DL, new Token(TokenType.Register, RegisterType.DL.ToString()));
            _pool.Add(PoolEntryType.DX, new Token(TokenType.Register, RegisterType.DX.ToString()));

            _pool.Add(PoolEntryType.ByteMemCell, new Token(TokenType.Identifier, SAMPLE_BYTE_MEMCELL));
            _pool.Add(PoolEntryType.WordMemCell, new Token(TokenType.Identifier, SAMPLE_WORD_MEMCELL));
            _pool.Add(PoolEntryType.Label, new Token(TokenType.Identifier, SAMPLE_LABEL));

            _pool.Add(PoolEntryType.ByteConst, new Token(TokenType.DecConstant, SAMPLE_BYTE_CONST));
            _pool.Add(PoolEntryType.WordConst, new Token(TokenType.DecConstant, SAMPLE_WORD_CONST));

        }

        public Token this[PoolEntryType entryType]
        {
            get {
                return _pool[entryType];
            }
        }
    }
}
