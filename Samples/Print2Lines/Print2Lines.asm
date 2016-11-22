        MOV   AX    , AB
        MOV   BX    , ZN

        MOV   CX    , AX
        MOV   AX    , BX
        MOV   BX    , CX

        MOV   PRINT , AX
        LEA   DX    , PRINT
        MOV   AH    , 9
        INT   21H

        MOV   CL    , BH
        MOV   BH    , BL
        MOV   BL    , CL

        MOV   PRINT , BX
        LEA   DX    , PRINT
        MOV   AH    , 9
        INT   21H
        JMP   END

        AB    DW    4241H
        ZN    DW    3C2BH
        PRINT DW    ?
        CR1   DB    13
        CR2   DB    10
        ENDL  DB    "$"

END:    XOR   AX  , AX
        XOR   BX  , BX
        XOR   CX  , CX
        XOR   DX  , DX
        MOV   AX  , 4C00H
        INT   21H