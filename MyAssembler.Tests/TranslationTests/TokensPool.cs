using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;

namespace MyAssembler.Tests.TranslationTests
{
    public enum PoolEntryType
    {
        MOV,
        ADD,
        SUB,
        AND,
        OR,
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

            _pool.Add(PoolEntryType.Comma, new Token(TokenType.Comma, ","));
            
            _pool.Add(PoolEntryType.AH, new Token(TokenType.Register, "AH"));
            _pool.Add(PoolEntryType.AL, new Token(TokenType.Register, "AL"));
            _pool.Add(PoolEntryType.AX, new Token(TokenType.Register, "AX"));
            _pool.Add(PoolEntryType.BH, new Token(TokenType.Register, "BH"));
            _pool.Add(PoolEntryType.BL, new Token(TokenType.Register, "BL"));
            _pool.Add(PoolEntryType.BX, new Token(TokenType.Register, "BX"));
            _pool.Add(PoolEntryType.CH, new Token(TokenType.Register, "CH"));
            _pool.Add(PoolEntryType.CL, new Token(TokenType.Register, "CL"));
            _pool.Add(PoolEntryType.CX, new Token(TokenType.Register, "CX"));
            _pool.Add(PoolEntryType.DH, new Token(TokenType.Register, "DH"));
            _pool.Add(PoolEntryType.DL, new Token(TokenType.Register, "DL"));
            _pool.Add(PoolEntryType.DX, new Token(TokenType.Register, "DX"));

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
