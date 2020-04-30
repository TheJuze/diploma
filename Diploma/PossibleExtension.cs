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
        /// <summary>
        /// Returns sorted PossibleExtensions
        /// </summary>
        /// <param name="omega"></param>
        /// <returns> </returns>
        public static List<PossibleExtension> FindPossibleExtensions(List<int> omega)
        {
            D Darray = new D();
            List<PossibleExtension> S = new List<PossibleExtension>();
            if (omega.Count == 1)
            {
                return S;
            }
            Darray = Matrix.FindD(omega);
            for (int i = 0; i < Darray.Index.Count; i++)
            {
                if (omega.Contains(Darray.i[i]) && omega.Contains(Darray.j[i]))
                {
                    //весовой коэф
                    #region
                    int currentWeight = 0;
                    for (int j = 0; j < Darray.Index[i].Count; j++)
                    {
                        foreach (WeightCoefficient weight in Program.weights)
                        {
                            if (weight.index == Darray.Index[i][j])
                            {
                                currentWeight += weight.weight;
                            }
                        }
                    }
                    #endregion
                    PossibleExtension s = new PossibleExtension(Darray.i[i], Darray.j[i], currentWeight, Darray.Index[i]);
                    S.Add(s);
                }
            }
            //sorting
            PossibleExtensionComparer sc = new PossibleExtensionComparer();
            S.Sort(sc);
            return S;
        }

        public static PossibleExtension FindKnot(List<PossibleExtension> possibleExtensions)
        {

            return possibleExtensions[0];
        }
    }
}
