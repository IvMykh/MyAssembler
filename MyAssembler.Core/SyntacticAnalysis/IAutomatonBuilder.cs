namespace MyAssembler.Core.SyntacticAnalysis
{
    public interface IAutomatonBuilder
    {
        IAutomatonNode ConstructedInstance { get; }
        void Construct();
    }
}
