using System;
using System.Collections.Generic;
using System.Text;

namespace Diploma
{
    public struct OrderedMultitude
    {
        public int weight { get; set; }
        public List<int> elements;
        public OrderedMultitude(int _weight, List<int> _elements)
        {
            this.weight = _weight;
            this.elements = new List<int>(_elements);

        }
    }
}
