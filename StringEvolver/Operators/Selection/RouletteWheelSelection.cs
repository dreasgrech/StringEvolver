using System;

namespace StringEvolver.Operators.Selection
{
    class RouletteWheelSelection:ISelection
    {
        private readonly Random random;

        public RouletteWheelSelection()
        {
            random = new Random();
        }

        public Chromosome Select(Population population)
        {
            var ran = random.NextDouble(0, population.Fitness);
            double sum = 0;
            foreach (var chromosome in population)
            {
                sum += chromosome.Fitness;
                if (sum > ran)
                {
                    return chromosome;
                }
            }
            throw new Exception("A chromosome should always be selected");
        }
    }
}
