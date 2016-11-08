namespace MyAssembler.Core.SyntacticAnalysis
{
    public enum OperandsSetType
    {
        NotAccepting,
        None,
        R,   M,   I,
        AR,  AM,
        RR,  RM,  MR,  RI,  MI,
        RRI, RMI
    }
}
