using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StringEvolver
{
    [DebuggerDisplay("{Fitness}")]
    class Population:IEnumerable<Chromosome>, IHasFitness
    {
        public double Fitness { private set; get; }
        public SortedChromosomeCollection Chromosomes { get; set; }

        public Population(IEnumerable<Chromosome> chromosomes)
        {
            Chromosomes = new SortedChromosomeCollection(chromosomes);
            Fitness = Chromosomes.Sum(c => c.Fitness);
        }

        public bool ContainsPerfectSolution()
        {
            return double.IsInfinity(Fitness);
        }
        public IEnumerator<Chromosome> GetEnumerator()
        {
            return Chromosomes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
