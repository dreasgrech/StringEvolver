using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using StringEvolver.FitnessCalculators;
using StringEvolver.Operators.Crossover;
using StringEvolver.Operators.Mutation;
using StringEvolver.CharacterGenerators;
using StringEvolver.Operators.Selection;

namespace StringEvolver
{
    class Program
    {
        //Rates (0-1)
        private const double CROSSOVER = 0.6,
                             MUTATION = 0.25,
                             ELITISM = 0.1;

        private const int CHROMOSOME_COUNT = 2000;
        private const int ELITISM_AMOUNT = ((int)(ELITISM * CHROMOSOME_COUNT));

        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var target = args[0];
            Console.WriteLine("Evolution Destination: {0}\n", target);

            var geneCount = target.Length;

            ICharacterGenerator characterGenerator = new ASCIICharacterGenerator();
            IFitnessCalculator fitness = new ByCharacterCalculator(target);
            ICrossover crossover = new OnePointCrossover(fitness);
            //ICrossover crossover = new TwoPointCrossover(fitness);
            IMutation mutation = new SingleSwitchMutator(characterGenerator, fitness);
            ISelection selection = new RouletteWheelSelection(fitness);

            var elementGenerator = new RandomElementFactory(characterGenerator,
                                                            geneCount,
                                                            CHROMOSOME_COUNT,
                                                            fitness);

            var before = DateTime.Now;
            Population population = elementGenerator.RandomPopulation();
            int totalGenerations = 0;

            do
            {
                totalGenerations++;
                population = AdvanceGeneration(population, selection, crossover, mutation);
                Console.WriteLine("{0,5}: {1}", totalGenerations, population.First().Value);
            } while (!double.IsInfinity(population.Fitness));

            var after = DateTime.Now;

            Console.WriteLine(String.Format("\nFOUND in {0} generations", totalGenerations));
            Console.WriteLine("Time Taken: {0}", (after - before));

            if (Debugger.IsAttached) //Running from the IDE
            {
                Console.ReadKey();
            }
        }

        static Population AdvanceGeneration(Population population, ISelection selection, ICrossover crossover, IMutation mutation)
        {
            var chromosomes = new List<Chromosome>();

            var elite = population.Take(ELITISM_AMOUNT);
            chromosomes.AddRange(elite);

            do
            {
                Chromosome chosen1 = selection.Select(population), chosen2 = selection.Select(population);

                if (random.NextDouble() < CROSSOVER)
                {
                    var children = crossover.Crossover(chosen1, chosen2);
                    chosen1 = children.Item1;
                    chosen2 = children.Item2;
                }

                if (random.NextDouble() < MUTATION)
                {
                    chosen1 = mutation.Mutate(chosen1);
                }

                if (random.NextDouble() < MUTATION)
                {
                    chosen2 = mutation.Mutate(chosen2);
                }

                chromosomes.Add(chosen1);
                chromosomes.Add(chosen2);
            } while (chromosomes.Count < CHROMOSOME_COUNT);

            return new Population(chromosomes);
        }
    }
}

