using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StringEvolver.Operators.Selection
{
    interface ISelection
    {
        Chromosome Select(Population population);
    }
}
