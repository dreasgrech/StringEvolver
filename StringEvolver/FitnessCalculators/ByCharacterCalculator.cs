using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringEvolver.FitnessCalculators
{
    class ByCharacterCalculator:IFitnessCalculator
    {
        private readonly string target;
        public ByCharacterCalculator(string target)
        {
            this.target = target;
            
        }
        public double CalculateFitness(Chromosome ch)
        {
            var distanceToTarget = 0;
            for (int i = 0; i < target.Length; i++)
            {
                distanceToTarget += Math.Abs(target[i] - ch.Value[i]);
            }
            return 1.0/distanceToTarget;
        }

        public double CalculateFitness(Population pop)
        {
            double total = 0;
            foreach (var chromosome in pop)
            {
                total += CalculateFitness(chromosome);
            }
            return total;
        }
    }
}
