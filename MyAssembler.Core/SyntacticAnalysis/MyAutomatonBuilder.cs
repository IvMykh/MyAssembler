using System;
using System.Collections.Generic;
using MyAssembler.Core.LexicalAnalysis;
using MyAssembler.Core.Properties;

namespace MyAssembler.Core.SyntacticAnalysis
{
    using OST = OperandsSetType;
    using TT  = TokenType;
    using DT  = DirectiveType;

    public class MyAutomatonBuilder
        : IAutomatonBuilder
    {
        private MyAutomatonNode _constructedInstance;
        
        public IAutomatonNode ConstructedInstance
        { 
            get {
                return _constructedInstance;
            } 
        }

        private MyAutomatonNode createForCommands(params Enum[] commandTypes)
        {
            var possibleInputs = new List<Enum>(commandTypes.Length);
            possibleInputs.AddRange(commandTypes);

            var root = new MyAutomatonNode(
                possibleInputs, OST.NotAccepting, Resources.Valid1stOpExpectedMsgFormat);

            return root;
        }
        private MyAutomatonNode createForDirectives(params Enum[] directiveTypes)
        {
            var possibleInputs = new List<Enum>(directiveTypes.Length);
            possibleInputs.AddRange(directiveTypes);

            var root = new MyAutomatonNode(
                possibleInputs, OST.NotAccepting, Resources.InitValueOrQuestionExpectedMsgFormat);

            return root;
        }
        private MyAutomatonNode createForDbMemCellInitializer()
        {
            var node = new MyAutomatonNode(
                new List<Enum> { 
                    TT.BinConstant, 
                    TT.DecConstant, 
                    TT.HexConstant, 
                    TT.Literal, 
                    TT.QuestionMark }, 
                OST.None, Resources.EndOfDtvExpectedMsgFormat);

            return node;
        }
        private MyAutomatonNode createForDwMemCellInitializer()
        {
            var node = new MyAutomatonNode(
                new List<Enum> { 
                    TT.BinConstant, 
                    TT.DecConstant, 
                    TT.HexConstant, 
                    TT.QuestionMark },
                OST.None, Resources.EndOfDtvExpectedMsgFormat);

            return node;
        }
        
        private MyAutomatonNode createForIdentifier()
        {
            var node = new MyAutomatonNode(
                new List<Enum> { TT.Identifier }, OST.NotAccepting, Resources.ColonDtvExpectedMsgFormat);

            return node;
        }
        private MyAutomatonNode createForColon()
        {
            var node = new MyAutomatonNode(
                new List<Enum> { TT.Colon }, OST.NotAccepting, Resources.CmdExpectedMsgFormat);

            return node;
        }

        private MyAutomatonNode createForRrRmRi()
        {
            var rootR = new MyAutomatonNode(
                new List<Enum> { TT.Register }, OST.NotAccepting, Resources.CommaExpectedMsgFormat);

            var rootRComma = new MyAutomatonNode(
                new List<Enum> { TT.Comma }, OST.NotAccepting, Resources.Valid2ndOpExpectedMsgFormat);

            var rootRCommaR = new MyAutomatonNode(
                new List<Enum> { TT.Register }, OST.RR, Resources.EndOfCmdExpectedMsgFormat);

            var rootRCommaM = new MyAutomatonNode(
                new List<Enum> { TT.Identifier }, OST.RM, Resources.EndOfCmdExpectedMsgFormat);

            var rootRCommaConst = new MyAutomatonNode(
                new List<Enum> { 
                    TT.BinConstant, 
                    TT.DecConstant, 
                    TT.HexConstant },
                    OST.RI, Resources.EndOfCmdExpectedMsgFormat);

            rootRComma.AddChildren(rootRCommaR, rootRCommaM, rootRCommaConst);
            rootR.AddChild(rootRComma);

            return rootR;
        }
        private MyAutomatonNode createForMrMi()
        {
            var rootM = new MyAutomatonNode(
                new List<Enum> { TT.Identifier }, OST.NotAccepting, Resources.CommaExpectedMsgFormat);

            var rootMComma = new MyAutomatonNode(
                new List<Enum> { TT.Comma }, OST.NotAccepting, Resources.Valid2ndOpExpectedMsgFormat);

            var rootMCommaR = new MyAutomatonNode(
                new List<Enum> { TT.Register }, OST.MR, Resources.EndOfCmdExpectedMsgFormat);

            var rootMCommaConst = new MyAutomatonNode(
                new List<Enum> { 
                    TT.BinConstant, 
                    TT.DecConstant, 
                    TT.HexConstant },
                    OST.MI, Resources.EndOfCmdExpectedMsgFormat);

            rootMComma.AddChildren(rootMCommaR, rootMCommaR, rootMCommaConst);
            rootM.AddChild(rootMComma);

            return rootM;
        }

        private MyAutomatonNode createForArRrRriRmRmi()
        {
            var rootR = new MyAutomatonNode(
                new List<Enum> { TT.Register }, OST.AR, Resources.CommaExpectedMsgFormat);

            var rootRComma = new MyAutomatonNode(
                new List<Enum> { TT.Comma }, OST.NotAccepting, Resources.Valid2ndOpExpectedMsgFormat);

            rootR.AddChild(rootRComma);

            // First branch.
            var rootRCommaR = new MyAutomatonNode(
                new List<Enum> { TT.Register }, OST.RR, Resources.CommaExpectedMsgFormat);

            var rootRCommaRComma = new MyAutomatonNode(
                new List<Enum> { TT.Comma }, OST.NotAccepting, Resources.Valid3rdOpExpectedMsgFormat);

            var rootRCommaRCommaI = new MyAutomatonNode(
                new List<Enum> { 
                    TT.BinConstant, 
                    TT.DecConstant, 
                    TT.HexConstant },
                    OST.RRI, Resources.EndOfCmdExpectedMsgFormat);

            rootRCommaR.AddChild(rootRCommaRComma);
            rootRCommaRComma.AddChild(rootRCommaRCommaI);

            // Second branch.
            var rootRCommaM = new MyAutomatonNode(
                new List<Enum> { TT.Identifier }, OST.RM, Resources.CommaExpectedMsgFormat);

            var rootRCommaMComma = new MyAutomatonNode(
                new List<Enum> { TT.Comma }, OST.NotAccepting, Resources.Valid3rdOpExpectedMsgFormat);

            var rootRCommaMCommaI = new MyAutomatonNode(
                new List<Enum> { 
                    TT.BinConstant, 
                    TT.DecConstant, 
                    TT.HexConstant },
                    OST.RMI, Resources.EndOfCmdExpectedMsgFormat);

            rootRCommaM.AddChild(rootRCommaMComma);
            rootRCommaMComma.AddChild(rootRCommaMCommaI);

            // Final.
            rootRComma.AddChildren(rootRCommaR, rootRCommaM);

            return rootR;
        }

        private MyAutomatonNode createForAR()
        {
            var rootR = new MyAutomatonNode(
                new List<Enum> { TT.Register }, OST.AR, Resources.EndOfCmdExpectedMsgFormat);

            return rootR;
        }
        private MyAutomatonNode createForAM()
        {
            var rootM = new MyAutomatonNode(
                new List<Enum> { TT.Identifier }, OST.AM, Resources.EndOfCmdExpectedMsgFormat);

            return rootM;
        }

        private MyAutomatonNode createForR()
        {
            var rootR = new MyAutomatonNode(
                new List<Enum> { TT.Register }, OST.R, Resources.EndOfCmdExpectedMsgFormat);

            return rootR;
        }
        private MyAutomatonNode createForM()
        {
            var rootM = new MyAutomatonNode(
                new List<Enum> { TT.Identifier }, OST.M, Resources.EndOfCmdExpectedMsgFormat);

            return rootM;
        }
        private MyAutomatonNode createForI()
        {
            var rootI = new MyAutomatonNode(
                new List<Enum> { 
                    TT.BinConstant, 
                    TT.DecConstant, 
                    TT.HexConstant
                }, OST.I, Resources.EndOfCmdExpectedMsgFormat);

            return rootI;
        }

        private MyAutomatonNode createForRm()
        {
            var rootR = new MyAutomatonNode(
                new List<Enum> { TT.Register }, OST.NotAccepting, Resources.CommaExpectedMsgFormat);

            var rootRComma = new MyAutomatonNode(
                new List<Enum> { TT.Comma }, OST.NotAccepting, Resources.Valid2ndOpExpectedMsgFormat);

            var rootRCommaM = new MyAutomatonNode(
                new List<Enum> { TT.Identifier }, OST.RM, Resources.EndOfCmdExpectedMsgFormat);

            rootRComma.AddChildren(rootRCommaM);
            rootR.AddChild(rootRComma);

            return rootR;
        }

        private void constructMovAddSubAndOrXor(MyAutomatonNode colonNode)
        {
            Enum[] commands = new Enum[] {
                CommandType.MOV, 
                CommandType.ADD, CommandType.SUB, 
                CommandType.AND, CommandType.OR, CommandType.XOR
            };

            var movAddSubAndOrXorNode = createForCommands(commands);
            movAddSubAndOrXorNode.AddChildren(createForRrRmRi(), createForMrMi());

            colonNode.AddChild(movAddSubAndOrXorNode);
            _constructedInstance.AddChild(movAddSubAndOrXorNode);
        }
        private void constructImul(MyAutomatonNode colonNode)
        {
            var imulNode = createForCommands(CommandType.IMUL);
            
            imulNode.AddChild(createForArRrRriRmRmi());
            imulNode.AddChild(createForAM());

            colonNode.AddChild(imulNode);
            _constructedInstance.AddChild(imulNode);
        }
        private void constructIdiv(MyAutomatonNode colonNode)
        {
            var idivNode = createForCommands(CommandType.IDIV);
            idivNode.AddChildren(createForAR(), createForAM());

            colonNode.AddChild(idivNode);
            _constructedInstance.AddChild(idivNode);
        }    
        private void constructNot(MyAutomatonNode colonNode)
        {
            var notNode = createForCommands(CommandType.NOT);
            notNode.AddChildren(createForR(), createForM());

            colonNode.AddChild(notNode);
            _constructedInstance.AddChild(notNode);
        }
        private void constructJmp(MyAutomatonNode colonNode)
        {
            var jmpNode = createForCommands(CommandType.JMP);
            jmpNode.AddChild(createForM());

            colonNode.AddChild(jmpNode);
            _constructedInstance.AddChild(jmpNode);
        }
        private void constructJe(MyAutomatonNode colonNode)
        {
            var jeNode = createForCommands(CommandType.JE);
            jeNode.AddChild(createForM());

            colonNode.AddChild(jeNode);
            _constructedInstance.AddChild(jeNode);
        }
        private void constructJne(MyAutomatonNode colonNode)
        {
            var jneNode = createForCommands(CommandType.JNE);
            jneNode.AddChild(createForM());

            colonNode.AddChild(jneNode);
            _constructedInstance.AddChild(jneNode);
        }
        private void constructInt(MyAutomatonNode colonNode)
        {
            var intNode = createForCommands(CommandType.INT);
            intNode.AddChild(createForI());

            colonNode.AddChild(intNode);
            _constructedInstance.AddChild(intNode);
        }
        private void constructLea(MyAutomatonNode colonNode)
        {
            var leaNode = createForCommands(CommandType.LEA);
            leaNode.AddChild(createForRm());

            colonNode.AddChild(leaNode);
            _constructedInstance.AddChild(leaNode);
        }

        public void Construct()
        {
            _constructedInstance = new MyAutomatonNode(
                null, OST.NotAccepting, Resources.IdfCmdExpectedMsgFormat);

            // Label:
            var labelNode = createForIdentifier();
            var colonNode = createForColon();
            
            //var directiveNode = createForDirectives(DT.DB, DT.DW);
            //directiveNode.AddChild(createForMemCellInitializer());
            //labelNode.AddChildren(colonNode, directiveNode);

            var dbNode = createForDirectives(DT.DB);
            dbNode.AddChild(createForDbMemCellInitializer());

            var dwNode = createForDirectives(DT.DW);
            dwNode.AddChild(createForDwMemCellInitializer());

            labelNode.AddChildren(colonNode, dbNode, dwNode);
            
            _constructedInstance.AddChild(labelNode);
            
            constructMovAddSubAndOrXor(colonNode);
            constructImul(colonNode);
            constructIdiv(colonNode);
            constructNot(colonNode);
            constructJmp(colonNode);
            constructJe(colonNode);
            constructJne(colonNode);
            constructInt(colonNode);
            constructLea(colonNode);
        }
    }
}
