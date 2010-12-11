using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StringEvolver.CharacterGenerators;
using StringEvolver.FitnessCalculators;

namespace StringEvolver
{
    class RandomElementFactory
    {
        private readonly ICharacterGenerator characterGenerator;
        private readonly FitnessCalculator fitnessCalculator;

        public int GeneCount { get; set; }
        public int ChromosomeCount { get; set; }

        public RandomElementFactory(ICharacterGenerator characterGenerator, int geneCount, int chromsomeCount, FitnessCalculator fitnessCalculator)
        {
            this.characterGenerator = characterGenerator;
            this.fitnessCalculator = fitnessCalculator;
            GeneCount = geneCount;
            ChromosomeCount = chromsomeCount;
        }

        public char RandomGene()
        {
            return characterGenerator.GenerateCharacter();
        }

        public Chromosome RandomChromosome()
        {
            var genes = new StringBuilder(GeneCount);
            for (int i = 0; i < GeneCount; i++)
            {
                genes.Append(RandomGene());
            }
            return new Chromosome(genes.ToString(), fitnessCalculator);
        }

        public Population RandomPopulation()
        {
            var chromosomes = new Chromosome[ChromosomeCount];
            for (int i = 0; i < ChromosomeCount; i++)
            {
                chromosomes[i] = RandomChromosome();
            }
            return new Population(chromosomes);
        }
    }
}
