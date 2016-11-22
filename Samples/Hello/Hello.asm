            JMP     start                       ; skip data definition
msg         DB      "Hello, assembler world!"   ; message
carrReturn  DB      13                          ; carriage return character
nextLine    DB      10                          ; next line symbol
endline     DB      "$"                         ; line terminator character

start:      MOV     AH  , 09                    ; print a line ended by '$'
            LEA     DX  , msg                   ; get address of message
            INT     21H                         ; call DOS procedure
            MOV     AX  , 4C00H                 
            INT     21H                         ; finish program