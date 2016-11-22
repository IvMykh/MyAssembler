        MOV     AH  , 09        ; print title message (ended by '$')
        LEA     DX  , msg       ; get address of message
        INT     21H             ; call DOS procedure        

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
        NL1   DB    10          ; Caret return symbol 2
        ENDL1 DB    "$"         ; Text delimiter

        msg   DB    "Print 2 lines: +< and BA"  ; message
        CR2   DB    13                          ; carriage return character
        NL2   DB    10                          ; next line symbol
        ENDL2 DB    "$"                         ; line terminator character

END:    XOR   AX  , AX          ; Clear registers
        XOR   BX  , BX
        XOR   CX  , CX
        XOR   DX  , DX

        MOV   AX  , 4C00H
        INT   21H               ; Finish program