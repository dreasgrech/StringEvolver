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
        private static FitnessCalculator fitness;

        private static readonly Random random = new Random();
        private static OptionSet options;

        static void Main(string[] args)
        {
            if (!HandleCommandLineArgs(args))
            {
                ShowHelp(options);
                return;
            }
           
            Console.WriteLine("\nEvolution Destination: {0}", target);
            Console.WriteLine("Mutation Rate: {0}%", mutationRate * 100);
            Console.WriteLine("Crossover Rate: {0}%", crossoverRate * 100);
            Console.WriteLine("Chromosomes / population: {0}", chromosomeCount);
            Console.WriteLine("Elitism / population: {0} ({1}%)", elitismRate * chromosomeCount, elitismRate);
            Console.WriteLine("Fitness Calculator: {0}", fitness);
            Console.WriteLine();

            ICharacterGenerator characterGenerator = new ASCIICharacterGenerator();
            //FitnessCalculator fitness = new ByCharacterCalculator(target);
            //FitnessCalculator fitness = new LevenshteinDistanceCalculator(target);
            ICrossover crossover = new OnePointCrossover(fitness);
            //ICrossover crossover = new TwoPointCrossover(fitness);
            IMutation mutation = new SingleSwitchMutator(characterGenerator, fitness);
            ISelection selection = new RouletteWheelSelection();

            var elementGenerator = new RandomElementFactory(characterGenerator, target.Length, chromosomeCount, fitness);

            var before = DateTime.Now;
            var population = elementGenerator.RandomPopulation();
            var totalGenerations = 1;

            while (true)
            {
                Console.WriteLine("{0,5}: {1}", totalGenerations, population.First().Value);
                if (population.ContainsOptimalSolution())
                {
                    break;
                }
                population = AdvanceGeneration(population, selection, crossover, mutation);
                totalGenerations++;
            }

            var after = DateTime.Now;

            Console.WriteLine(String.Format("\nFound in {0} generation{1}", totalGenerations, totalGenerations > 1 ? "s" : ""));
            Console.WriteLine("Time Taken: {0}", (after - before));

            if (Debugger.IsAttached) //Running from the IDE
            {
                Console.ReadKey();
            }
        }

        static Population AdvanceGeneration(Population population, ISelection selection, ICrossover crossover, IMutation mutation)
        {
            var chromosomes = new List<Chromosome>();
            chromosomes.AddRange(population.Take((int)(elitismRate * chromosomeCount)));  //ELITE

            do
            {
                Chromosome chosen1 = selection.Select(population),
                           chosen2 = selection.Select(population);

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

        static bool HandleCommandLineArgs(IEnumerable<string> args)
        {
            string fitnessType = "";
            bool status = true;
            options = new OptionSet
                          {
                              {"m|mutation=", "The mutation rate (0-1)", (double v) => mutationRate = v},
                              {"s|crossover=", "The crossover rate (0-1)", (double v) => crossoverRate = v},
                              {"e|elitism=", "The elitism rate (0-1)", (double v) => elitismRate = v},
                              {"c|crcount=", "The number of chromosomes per population (>0)", (int v) => chromosomeCount = v},
                              {"fitness=", "The fitness calculator [sum | levenshtein]", v => fitnessType = v},
                              {"?|h|help", "Show help", v => { status = false; }},
                              {"<>", v => target = v}
                          };
            try
            {
                options.Parse(args);
            }
            catch (OptionException ex)
            {
                status = false;
            }

            fitness = (FitnessCalculator)Activator.CreateInstance(GetFitnessCalculatorType(fitnessType), new object[] { target });

            if (String.IsNullOrEmpty(target) || mutationRate > 1 || crossoverRate > 1 || elitismRate > 1 || chromosomeCount <= 0)
            {
                status = false;
            }

            return status;
        }

        static void ShowHelp(OptionSet options)
        {
            Console.WriteLine("Usage: {0} [options] <destination>\n", AppDomain.CurrentDomain.FriendlyName);
            Console.WriteLine("Options:");
            options.WriteOptionDescriptions(Console.Out);
        }

        static Type GetFitnessCalculatorType(string arg)
        {
            arg = arg.ToLower();
            var types = new Dictionary<string, Type>
                            {
                                {"sum", typeof (ByCharacterCalculator)},
                                {"levenshtein", typeof (LevenshteinDistanceCalculator)}
                            };
            return types.ContainsKey(arg) ? types[arg] : types.First().Value;
        }
    }
}

