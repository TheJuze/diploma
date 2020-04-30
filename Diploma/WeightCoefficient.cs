using System;
using System.Collections.Generic;
using System.Text;

namespace Diploma
{
    public struct WeightCoefficient
    {
        public int index { get; set; }
        public int weight { get; set; }
        public WeightCoefficient(int _index, int _weight)
        {
            this.index = _index;
            this.weight = _weight;
        }
    }
    
}
