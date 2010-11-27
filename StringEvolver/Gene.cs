using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StringEvolver
{
    [DebuggerDisplay("{Value}")]
    class Gene
    {
        public char Value { get; set; }

        public Gene(char value)
        {
            Value = value;
        }
    }
}
