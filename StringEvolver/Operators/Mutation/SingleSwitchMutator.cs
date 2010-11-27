using System;
using StringEvolver.CharacterGenerators;
using StringEvolver.FitnessCalculators;

namespace StringEvolver.Operators.Mutation
{
    class SingleSwitchMutator:IMutation
    {
        private Random random;
        private ICharacterGenerator generator;
        private IFitnessCalculator fitnessCalculator;

        public SingleSwitchMutator(ICharacterGenerator characterGenerator, IFitnessCalculator fitnessCalculator)
        {
            this.fitnessCalculator = fitnessCalculator;
            random = new Random();
            generator = characterGenerator;
        }

        public Chromosome Mutate(Chromosome ch)
        {
            var randomPositon = random.Next(0, ch.Value.Length - 1);
            return
                new Chromosome(ch.Value.Substring(0, randomPositon) + generator.GenerateCharacter() +
                               ch.Value.Substring(randomPositon + 1),fitnessCalculator);
        }
    }
}
