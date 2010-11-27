using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StringEvolver.FitnessCalculators;

namespace StringEvolver.Operators.Crossover
{
    class TwoPointCrossover:ICrossover
    {
         private readonly Random random;
        private IFitnessCalculator fitnessCalculator;

        public TwoPointCrossover(IFitnessCalculator fitnessCalculator)
        {
            this.fitnessCalculator = fitnessCalculator;
            random = new Random();
        }

        public Tuple<Chromosome, Chromosome> Crossover(Chromosome c1, Chromosome c2)
        {
            //TODO: not yet working
            int locus1 = random.Next(0, c1.Value.Length - 1), locus2 = random.Next(locus1 + 1, c1.Value.Length), distance = locus2 - locus1;
            IEnumerable<Gene> ch1 = c1.Take(locus1).Concat(c1.Skip(locus1).Take(distance)).Concat(c1.Skip(locus2)),
                              ch2 = c2.Take(locus1).Concat(c2.Skip(locus1).Take(distance)).Concat(c2.Skip(locus2));
            return new Tuple<Chromosome, Chromosome>(new Chromosome(ch1, fitnessCalculator), new Chromosome(ch2, fitnessCalculator));
        }
    }
}
