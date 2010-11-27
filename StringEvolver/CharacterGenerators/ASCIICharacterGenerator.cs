using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringEvolver.CharacterGenerators
{
    class ASCIICharacterGenerator:ICharacterGenerator
    {
        private readonly Random random;

        public ASCIICharacterGenerator()
        {
            random = new Random();
        }
        public char GenerateCharacter()
        {
            return (char)random.Next(32, 126);
        }
    }
}
