using System;
using System.Collections.Generic;
using System.Linq;
using StringEvolver.FitnessCalculators;

namespace StringEvolver.Operators.Crossover
{
    class OnePointCrossover:ICrossover
    {
        private readonly Random random;
        private IFitnessCalculator fitnessCalculator;

        public OnePointCrossover(IFitnessCalculator fitnessCalculator)
        {
            this.fitnessCalculator = fitnessCalculator;
            random = new Random();
        }

        public Tuple<Chromosome, Chromosome> Crossover(Chromosome c1, Chromosome c2)
        {
            var locus = random.Next(0, c1.Value.Length); // Locus: http://en.wikipedia.org/wiki/Locus_(genetics)
            IEnumerable<Gene> ch1 = c1.Take(locus).Concat(c2.Skip(locus)),
                              ch2 = c2.Take(locus).Concat(c1.Skip(locus));
            return new Tuple<Chromosome, Chromosome>(new Chromosome(ch1, fitnessCalculator), new Chromosome(ch2, fitnessCalculator));
        }
    }
}
