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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newOmega">omega.except thomthing</param>
        /// <param name="weights"> weight coef</param>
        /// <returns> Central Element</returns>
        public static CentralElement FindCentral(List<int> newOmega)
        {
            CentralElement centralElement = new CentralElement(-1, 0);
            foreach (WeightCoefficient _weight in Program.weights)
            {
                if (newOmega.Contains(_weight.index) && centralElement.weight < _weight.weight)
                {
                    centralElement.index = _weight.index;
                    centralElement.weight = _weight.weight;
                }
            }

            return centralElement;
        }
    }
}
