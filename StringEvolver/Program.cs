using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using StringEvolver.CommandLineArgs;
using StringEvolver.FitnessCalculators;
using StringEvolver.Operators.Crossover;
using StringEvolver.Operators.Mutation;
using StringEvolver.CharacterGenerators;
using StringEvolver.Operators.Selection;

namespace StringEvolver
{
    class Program
    {

        private static double crossoverRate = 0.6, mutationRate = 0.25, elitismRate = 0.1;
        private static int chromosomeCount = 2000;

        private static string target = "";

        private static readonly Random random = new Random();
        private static OptionSet options;

        static bool HandleCommandLineArgs(IEnumerable<string> args)
        {
            bool status = true;
            options = new OptionSet
                        {
                            {"m|mutation=", "The mutation rate", (double v) => mutationRate = v},
                            {"s|crossover=", "The crossover rate", (double v) => crossoverRate = v},
                            {"e|elitism=", "The elitism rate", (double v) => elitismRate = v},
                            {"c|crcount=", "The number of chromosomes per population", (int v) => chromosomeCount = v},
                            {"<>", v => target = v}
                        };

            options.Add("?|h|help", "Show help", v =>
                                  {
                                      status = false;
                                  });

            try
            {
                options.Parse(args);
                if (String.IsNullOrEmpty(target))
                {
                    status = false;
                }
            }
            catch (OptionException ex)
            {
                status = false;
            }

            return status;
        }

        static void Main(string[] args)
        {
            if (!HandleCommandLineArgs(args))
            {
                ShowHelp(options);
                return;
            }
           
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
                                                            chromosomeCount,
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

            var elite = population.Take((int)(elitismRate * chromosomeCount));
            chromosomes.AddRange(elite);

            do
            {
                Chromosome chosen1 = selection.Select(population), chosen2 = selection.Select(population);

                if (random.NextDouble() < crossoverRate)
                {
                    var children = crossover.Crossover(chosen1, chosen2);
                    chosen1 = children.Item1;
                    chosen2 = children.Item2;
                }

                if (random.NextDouble() < mutationRate)
                {
                    chosen1 = mutation.Mutate(chosen1);
                }

                if (random.NextDouble() < mutationRate)
                {
                    chosen2 = mutation.Mutate(chosen2);
                }

                chromosomes.Add(chosen1);
                chromosomes.Add(chosen2);
            } while (chromosomes.Count < chromosomeCount);

            return new Population(chromosomes);
        }

        static void ShowHelp(OptionSet options)
        {
            Console.WriteLine("Usage: {0} [options] <destination>\n", AppDomain.CurrentDomain.FriendlyName);
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
            
        }
    }
}

