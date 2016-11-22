            JMP     start                        ; skip data definition
			
msg         DB      "Calculate ((3+1)-(10/5))*3" ; message
carrReturn  DB      13                           ; carriage return character
nextLine    DB      10                           ; next line symbol
endline     DB      "$"                          ; line terminator character

output      DB      ?
carrReturn2 DB      13                           ; carriage return character
nextLine2   DB      10                           ; next line symbol
endline2    DB      "$"  

start:      MOV     AH  , 09                     ; print a line ended by '$'
            LEA     DX  , msg                    ; get address of message
            INT     21H                          ; call DOS procedure
            
            MOV     BX  , 3                      ; store 3
            ADD     BX  , 1                      ; add 1

            XOR     DX  , DX                     ; send 0 to DX
            MOV     AX  , 10                     ; store 10
            MOV     CX  , 5                      ; store 5
            IDIV    CX                           ; divide 10 / 5
            
            SUB     BX  , AX                     ; (3+1) - (10/5)
            MOV     AX  , BX                     ; store the result in AX

            MOV     CX  , 3                      ; store 3
            IMUL    CX                           ; ((3+1)-(10/5))*3

            MOV     output, AL                   ; store the result in output
            ADD     output, 48                   ; make character from digit

            MOV     AH  , 09                     ; print a line ended by '$'
            LEA     DX  , output                 ; get address of message
            INT     21H                          ; call DOS function

            MOV     AX  , 4C00H                 
            INT     21H                          ; finish program