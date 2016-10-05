namespace MyAssembler.Core.SyntacticAnalysis
{
    public enum OperandsSetType
    {
        NotAccepting,
        None,
        R,   M,
        AR,  AM,
        RR,  RM,  MR,  RI,  MI,
        RRI, RMI
    }
}
