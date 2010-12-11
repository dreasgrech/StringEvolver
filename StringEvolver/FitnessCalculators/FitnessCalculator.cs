using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringEvolver.FitnessCalculators
{
    abstract class FitnessCalculator
    {
        public string Target { get; set; }

        protected FitnessCalculator(string target)
        {
            Target = target;
        }

        public abstract double CalculateFitness(Chromosome ch);
        public double Calculatefitness(Population pop)
        {
            return pop.Sum(chromosome => CalculateFitness(chromosome));
        }
    }
}
