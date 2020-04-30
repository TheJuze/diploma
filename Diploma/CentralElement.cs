using System;
using System.Collections.Generic;
using System.Text;

namespace Diploma
{
    struct CentralElement
    {
        public int index { get; set; }
        public int weight { get; set; }
        public CentralElement(int _index, int _weight)
        {
            this.index = _index;
            this.weight = _weight;
        }
    }
}
