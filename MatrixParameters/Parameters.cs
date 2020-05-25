using System;
using System.IO;
using System.Linq;

namespace MatrixParameters
{
    class Parameters
    {
        private static int n;
        private static double[,] A;

        static void Main(string[] args)
        {
            n = Diploma.Program.n;
            A = new double[n, n];
            Fill();
          //  Console.WriteLine("Определитель={0}", A.GetDeterminant());
            Console.WriteLine("Плотность={0}", GetDensity());
        }
        private static void Fill()//Заполнить исходную матрицу из файла
        {//@"C:\Users\diman\source\repos\Diploma\input.txt"
            //Program.inputFileName
            double[] arr = File.ReadAllText(@"C:\Users\diman\source\repos\Diploma\input.txt").Split(' ', '\n').Select(n => double.Parse(n)).ToArray();
            //  int counter = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    A[i, j] = arr[i * n + j];
                    //     counter++;
                }
            }
        }
        private static double GetDensity()
        {
            double edge = 0;
            foreach (double element in A)
            {
                edge += element;
            }
            return edge/(n*n);
        }
    }
  /*  public static class MatrixHelper
    {
        public static unsafe double GetDeterminant(this double[,] a)
        {
            if (a.GetLength(0) != a.GetLength(1))
                throw new ArgumentException("Матрица должна быть квадратной!", "a");
            var temp = new double[a.Length];
            Buffer.BlockCopy(a, 0, temp, 0, temp.Length * sizeof(double));
            fixed (double* pm = &temp[0])
            {
                return _Det(pm, a.GetLength(0));
            }
        }

        unsafe static double _Det(double* rmX, int n)
        {
            double* mtx_u_ii, mtx_ii_j;
            double* mtx_end = rmX + n * (n - 1), mtx_u_ii_j;
            double val, det = 1;
            int d = 0;
            // rmX указывает на (i,i) элемент на каждом шаге и называется ведущим
            for (double* mtx_ii_end = rmX + n; rmX < mtx_end; rmX += n + 1, mtx_ii_end += n, d++)
            {
                // Ищем максимальный элемент в столбце(под ведущим) 
                {
                    //Ищем максимальный элемент и его позицию
                    val = Math.Abs(*(mtx_ii_j = rmX));
                    for (mtx_u_ii = rmX + n; mtx_u_ii < mtx_end; mtx_u_ii += n)
                    {
                        if (val < Math.Abs(*mtx_u_ii))
                            val = Math.Abs(*(mtx_ii_j = mtx_u_ii));
                    }
                    //Если максимальный эдемент = 0 -> матрица вырожденная
                    if (Math.Abs(val - 0) < double.Epsilon) return double.NaN;
                    //Если ведущий элемент не является максимальным - делаем перестановку строк и меняем знак определителя
                    if (mtx_ii_j != rmX)
                    {
                        det = -det;
                        for (mtx_u_ii = rmX; mtx_u_ii < mtx_ii_end; mtx_ii_j++, mtx_u_ii++)
                        {
                            val = *mtx_u_ii;
                            *mtx_u_ii = *mtx_ii_j;
                            *mtx_ii_j = val;
                        }
                    }
                }
                //Обнуляем элементы под ведущим
                for (mtx_u_ii = rmX + n, mtx_u_ii_j = mtx_end + n; mtx_u_ii < mtx_u_ii_j; mtx_u_ii += d)
                {
                    val = *(mtx_u_ii++) / *rmX;
                    for (mtx_ii_j = rmX + 1; mtx_ii_j < mtx_ii_end; mtx_u_ii++, mtx_ii_j++)
                        *mtx_u_ii -= *mtx_ii_j * val;
                }
                det *= *rmX;
            }
            return *rmX * det;
        }
    }*/
}
