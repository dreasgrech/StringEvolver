using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StringEvolver.FitnessCalculators;

namespace StringEvolver.Operators.Crossover
{
    class TwoPointCrossover : ICrossover
    {
        private readonly Random random;
        private FitnessCalculator fitnessCalculator;

        public TwoPointCrossover(FitnessCalculator fitnessCalculator)
        {
            this.fitnessCalculator = fitnessCalculator;
            random = new Random();
        }

        public Tuple<Chromosome, Chromosome> Crossover(Chromosome c1, Chromosome c2)
        {
            int locus1 = random.Next(0, c1.Value.Length),
                locus2 = random.Next(locus1, c1.Value.Length),
                distance = locus2 - locus1;

            string ch1 = c1.Value.Substring(0, locus1) + c2.Value.Substring(locus1, distance) + c1.Value.Substring(locus2),
                   ch2 = c2.Value.Substring(0, locus1) + c1.Value.Substring(locus1, distance) + c2.Value.Substring(locus2);

            return new Tuple<Chromosome, Chromosome>(new Chromosome(ch1, fitnessCalculator), new Chromosome(ch2, fitnessCalculator));
        }

        public override string ToString()
        {
            return "Two Point";
        }
    }
}
