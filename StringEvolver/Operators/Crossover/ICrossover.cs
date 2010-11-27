using System;

namespace StringEvolver.Operators.Crossover
{
    interface ICrossover
    {
        Tuple<Chromosome, Chromosome> Crossover(Chromosome c1, Chromosome c2);
    }
}
