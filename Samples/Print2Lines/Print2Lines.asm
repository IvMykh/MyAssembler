        MOV   AX    , AB        ; read A and B to AX
        MOV   BX    , ZN        ; read + and < to BX

        MOV   CX    , AX        ; exchange AX and BX contents
        MOV   AX    , BX
        MOV   BX    , CX

        MOV   PRINT , AX        ; + and < signs
        LEA   DX    , PRINT     ; line address
        MOV   AH    , 9         ; DOS function - print a line to screen
        INT   21H               ; function call

        MOV   CL    , BH        ; exchange BH and BL contents
        MOV   BH    , BL
        MOV   BL    , CL

        MOV   PRINT , BX        ; B and A letters
        LEA   DX    , PRINT     ; line address
        MOV   AH    , 9         ; DOS function - print a line to screen
        INT   21H               ; function call

        JMP   END               ; jump over data definitions

        AB    DW    4241H       ; codes of letters A(41H) and B(42H)
        ZN    DW    3C2BH       ; codes of signs +(2BH) and <(3CH)
        PRINT DW    ?           ; 2 letters for printing
        
        CR1   DB    13          ; Caret return symbol 1
        CR2   DB    10          ; Caret return symbol 2
        ENDL  DB    "$"         ; Text delimiter

END:    XOR   AX  , AX          ; Clear registers
        XOR   BX  , BX
        XOR   CX  , CX
        XOR   DX  , DX

        MOV   AX  , 4C00H
        INT   21H               ; Finish program