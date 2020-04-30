using System;
using System.Collections.Generic;
using System.Text;

namespace Diploma
{
    struct PossibleExtension
    {
        public int i;
        public int j;
        public int weight;
        public List<int> elements;
        public PossibleExtension(int _i, int _j, int _weight, List<int> _elements)
        {
            this.i = _i;
            this.j = _j;
            this.weight = _weight;
            this.elements = new List<int>(_elements);
            
        }
    }
}
