namespace StringEvolver.FitnessCalculators
{
    interface IFitnessCalculator
    {
        double CalculateFitness(Chromosome ch);
        double CalculateFitness(Population pop);
    }
}
