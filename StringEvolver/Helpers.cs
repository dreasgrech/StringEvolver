using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringEvolver
{
    public static class Helpers
    {
        public static double NextDouble(this Random rng, double min, double max)
        {
            return min + (rng.NextDouble() * (max - min));
        }
    }
}
