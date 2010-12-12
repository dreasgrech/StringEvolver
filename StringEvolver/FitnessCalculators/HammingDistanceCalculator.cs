using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringEvolver.FitnessCalculators
{
    class HammingDistanceCalculator:FitnessCalculator
    {
        public HammingDistanceCalculator(string target) : base(target)
        {
        }

        public override double CalculateFitness(Chromosome ch)
        {
            if (ch.Value.Length != Target.Length)
            {
                //throw new Exception("Hamming Distance requires both strings to be of equal length");
                return 1.0/double.PositiveInfinity;
            }

            var difference = 0;
            for (int i = 0; i < Target.Length; i++)
            {
                if (ch.Value[i] != Target[i])
                {
                    difference++;
                }
            }
            return 1.0/difference;
        }

        public override string ToString()
        {
            return "Hamming Distance";
        }
    }
}
