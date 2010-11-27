using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringEvolver
{
    class SortedChromosomeCollection:IEnumerable<Chromosome>
    {
        private List<Chromosome> chromosomes;

        public SortedChromosomeCollection(IEnumerable<Chromosome> chs)
        {
            BinaryHeap<Chromosome, Chromosome> heap = new BinaryHeap<Chromosome, Chromosome>();
            foreach (var chromosome in chs)
            {
                heap.Insert(chromosome, chromosome);
            }
            chromosomes = new List<Chromosome>(heap.Count);
            var totalChromosomes = heap.Count;
            for (int i = 0; i < totalChromosomes; i++)
            {
                chromosomes.Add(heap.RemoveTop());
            }
        }

        public IEnumerator<Chromosome> GetEnumerator()
        {
            return chromosomes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
