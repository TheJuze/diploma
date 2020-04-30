using System;
using System.Collections.Generic;
using System.Text;

namespace Diploma
{
    class PossibleExtensionComparer : IComparer<PossibleExtension>//сортировка для S
    {
        public int Compare(PossibleExtension x, PossibleExtension y)
        {
            if (x.weight > y.weight)
            {
                return -1;
            }
            else if (x.weight < y.weight)
            {
                return 1;
            }
            else if (x.i > y.i)
            {
                return 1;
            }
            else if (x.i < y.i)
            {
                return -1;
            }
            else if (x.j > y.j)
            {
                return 1;
            }
            else if (x.j < y.j)
            {
                return -1;
            }
            return 0;
        }
    }
}
