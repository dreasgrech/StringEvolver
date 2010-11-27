using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringEvolver.CharacterGenerators
{
    class AlphabeticalCharacterGenerator:ICharacterGenerator
    {
        private readonly Random random;

        public AlphabeticalCharacterGenerator()
        {
            random = new Random();
        }

        public char GenerateCharacter()
        {
            var upperLower = new[]
                                 {
                                     random.Next(65, 90), // a-z
                                     random.Next(97, 122) // A-Z
                                 };
            return (char)upperLower[random.Next(0, upperLower.Length)];
        }
    }
}
