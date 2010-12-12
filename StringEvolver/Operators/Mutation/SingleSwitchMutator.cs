using System;
using System.Text;
using StringEvolver.CharacterGenerators;
using StringEvolver.FitnessCalculators;

namespace StringEvolver.Operators.Mutation
{
    class SingleSwitchMutator:IMutation
    {
        private Random random;
        private ICharacterGenerator generator;
        private FitnessCalculator fitnessCalculator;

        public SingleSwitchMutator(ICharacterGenerator characterGenerator, FitnessCalculator fitnessCalculator)
        {
            this.fitnessCalculator = fitnessCalculator;
            random = new Random();
            generator = characterGenerator;
        }

        public Chromosome Mutate(Chromosome ch)
        {
            var sb = new StringBuilder(ch.Value);
            var randomPositon = random.Next(0, ch.Value.Length);
            sb[randomPositon] = generator.GenerateCharacter();

            return new Chromosome(sb.ToString(), fitnessCalculator);
        }

    }
}
