using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringEvolver.FitnessCalculators
{
    class ByCharacterCalculator:FitnessCalculator
    {
        public ByCharacterCalculator(string target):base(target){}

        public override double CalculateFitness(Chromosome ch)
        {
            var distanceToTarget = Target.Select((t, i) => Math.Abs(t - ch.Value[i])).Sum();
            return 1.0/distanceToTarget;
        }
    }
}
