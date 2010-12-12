using System;
using System.Collections.Generic;
using System.Linq;
using StringEvolver.FitnessCalculators;

namespace StringEvolver.Operators.Crossover
{
    class OnePointCrossover:ICrossover
    {
        private readonly Random random;
        private FitnessCalculator fitnessCalculator;

        public OnePointCrossover(FitnessCalculator fitnessCalculator)
        {
            this.fitnessCalculator = fitnessCalculator;
            random = new Random();
        }

        public Tuple<Chromosome, Chromosome> Crossover(Chromosome c1, Chromosome c2)
        {
            var locus = random.Next(0, c1.Value.Length + 1); // Locus: http://en.wikipedia.org/wiki/Locus_(genetics)
            string ch1 = c1.Value.Substring(0, locus) + c2.Value.Substring(locus),
                   ch2 = c2.Value.Substring(0, locus) + c1.Value.Substring(locus);
            
            return new Tuple<Chromosome, Chromosome>(new Chromosome(ch1, fitnessCalculator), new Chromosome(ch2, fitnessCalculator));
        }

        public override string ToString()
        {
            return "One Point";
        }
    }
}
