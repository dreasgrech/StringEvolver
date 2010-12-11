using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using StringEvolver.FitnessCalculators;

namespace StringEvolver
{
    [DebuggerDisplay("{Fitness}")]
    class Chromosome:IEnumerable<char>, IComparable<Chromosome>, IHasFitness
    {
        public double Fitness { private set; get; }

        public string Value { private set; get; }

        public Chromosome(string genes, FitnessCalculator fitnessCalculator)
        {
            Value = genes;
            Fitness = fitnessCalculator.CalculateFitness(this);
        }

        public IEnumerator<char> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int CompareTo(Chromosome other)
        {
            var diff = Fitness - other.Fitness;
            return diff > 0 ? -1 : diff < 0 ? 1 : 0;
        }
    }
}
