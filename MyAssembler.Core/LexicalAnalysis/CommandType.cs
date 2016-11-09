namespace MyAssembler.Core.LexicalAnalysis
{
    public enum CommandType
    {
        None,
        MOV,
        ADD,   SUB,  IMUL,  IDIV,
        AND,   OR,   NOT,   XOR,
        JMP,   JE,   JNE,
        INT,
        LEA
    }
}
