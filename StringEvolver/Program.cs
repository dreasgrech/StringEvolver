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

        private static double crossoverRate = 0.6, mutationRate = 0.25, elitismRate = 0.1, truncationRate = 0.8;
        private static int chromosomeCount = 2000;
        private static string target = "";

        private static FitnessCalculator fitness;
        private static ICrossover crossover;
        private static IMutation mutation;
        private static ISelection selection;
        private static ICharacterGenerator characterGenerator;

        private static readonly Random random = new Random();
        private static OptionSet options;

        private static Dictionary<string, Type> fitnessTypes = new Dictionary<string, Type>
                            {
                                {"hamming", typeof (HammingDistanceCalculator)},
                                {"sum", typeof (ByCharacterCalculator)},
                                {"levenshtein", typeof (LevenshteinDistanceCalculator)}
                            };

        private static Dictionary<string, Type> crossoverTypes = new Dictionary<string, Type>
                                                              {
                                                                  {"one", typeof (OnePointCrossover)},
                                                                  {"two", typeof (TwoPointCrossover)}
                                                              };

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
            Console.WriteLine("Truncation Rate: {0}%", truncationRate * 100);
            Console.WriteLine("Chromosomes / population: {0}", chromosomeCount);
            Console.WriteLine("Elitism / population: {0} ({1}%)", elitismRate * chromosomeCount, elitismRate);
            Console.WriteLine("Fitness Calculator: {0}", fitness);
            Console.WriteLine("Crossover Type: {0}", crossover);
            Console.WriteLine();

            var elementGenerator = new RandomElementFactory(characterGenerator, target.Length, chromosomeCount, fitness);

            var before = DateTime.Now;
            var population = elementGenerator.RandomPopulation(); // Generating the initial population
            var totalGenerations = 1;

            //TODO: Refactor the below while (true) loop
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
            population = new Population(population.Take((int)(truncationRate * population.Count()))); // TRUNCATION
            chromosomes.AddRange(population.Take((int)(elitismRate * chromosomeCount)));  //ELITE (assuming that the chromosomes in the population are sorted by fitness (the fitter are at the top of the list)

            do
            {
                Chromosome chosen1 = selection.Select(population),
                           chosen2 = selection.Select(population);

                if (random.NextDouble() < crossoverRate)
                {
                    var children = crossover.Crossover(chosen1, chosen2); // CROSSOVER
                    chosen1 = children.Item1;
                    chosen2 = children.Item2;
                }

                if (random.NextDouble() < mutationRate)
                {
                    chosen1 = mutation.Mutate(chosen1); // MUTATION
                }

                if (random.NextDouble() < mutationRate)
                {
                    chosen2 = mutation.Mutate(chosen2); // MUTATION
                }

                chromosomes.Add(chosen1);
                chromosomes.Add(chosen2);
            } while (chromosomes.Count < chromosomeCount);

            return new Population(chromosomes);
        }

        static bool HandleCommandLineArgs(IEnumerable<string> args)
        {
            string fitnessType = "", crossoverType = "";
            bool status = true;
            options = new OptionSet
                          {
                              {"m|mutation=", "The mutation rate (0-1)", (double v) => mutationRate = v},
                              {"s|crossover=", "The crossover rate (0-1)", (double v) => crossoverRate = v},
                              {"e|elitism=", "The elitism rate (0-1)", (double v) => elitismRate = v},
                              {"c|crcount=", "The number of chromosomes per population (>1)", (int v) => chromosomeCount = v},
                              {"fitness=", "The fitness calculator [sum | levenshtein | hamming]", v => fitnessType = v},
                              {"ctype=", "The crossover type [one | two ]", v => crossoverType = v},
                              {"t|truncate=", "The rate of the chromosomes to keep from a population before advancing (0 < t <= 1)", (double v) => truncationRate = v},
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

            fitness = (FitnessCalculator)Activator.CreateInstance(GetOperationType(fitnessTypes, fitnessType), new object[] { target });
            crossover = (ICrossover)Activator.CreateInstance(GetOperationType(crossoverTypes, crossoverType), new object[] { fitness });
            characterGenerator = new ASCIICharacterGenerator();
            mutation = new SinglePointMutation(characterGenerator, fitness);
            selection = new RouletteWheelSelection();

            if (String.IsNullOrEmpty(target) || mutationRate > 1 || crossoverRate > 1 || elitismRate > 1 || chromosomeCount <= 1 || truncationRate <= 0 || truncationRate > 1)
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

        static Type GetOperationType(Dictionary<string, Type> types, string argument)
        {
            argument = argument.ToLower();
            return types.ContainsKey(argument) ? types[argument] : types.First().Value;
        }
    }
}

