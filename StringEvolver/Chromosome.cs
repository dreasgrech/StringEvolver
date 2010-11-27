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
    class Chromosome:IEnumerable<Gene>, IComparable<Chromosome>, IHasFitness
    {
        public IEnumerable<Gene> Genes { get; set; }
        public double Fitness { private set; get; }

        public string Value { private set; get; }

        public Chromosome(IEnumerable<char> value, IFitnessCalculator fitnessCalculator) : this(value.Select(c => new Gene(c)), fitnessCalculator) { }
        public Chromosome(IEnumerable<Gene> genes, IFitnessCalculator fitnessCalculator)
        {
            Genes = genes;
            Value = String.Concat(Genes.Select(g => g.Value));
            Fitness = fitnessCalculator.CalculateFitness(this);
        }

        public IEnumerator<Gene> GetEnumerator()
        {
            return Genes.GetEnumerator();
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
