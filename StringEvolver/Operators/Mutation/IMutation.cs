namespace StringEvolver.Operators.Mutation
{
    interface IMutation
    {
        Chromosome Mutate(Chromosome ch);
    }
}
